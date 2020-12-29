using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Commands;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Commands {
    public class SetValueCommandMiddleware : Middleware, IGpsWatchCommandMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        private readonly AppSettings _appSettings;
        public SetValueCommandMiddleware(ITrackerRepository trackerRepository, IOptions<AppSettings> appSettings) {
            _trackerRepository = trackerRepository;
            _appSettings = appSettings.Value;
        }

        public override bool OperateOnMessage(PipelineContext context) {
            try {

                #region VALIDATION:

                (var isValid, var validationError) = CommandHelper.DoBasicValidation(context, _trackerRepository);
                var requestBytes = Convert.FromBase64String(context.Message.Base64Text);
                var request = CommandRequest.Deserialize(requestBytes);


                if (!isValid) {
                    var errorResponse = new CommandResponse { Done = false, Error = validationError };
                    context.Message.Socket.Send(errorResponse.Serialize());
                    return false;
                }
                #endregion

                #region PROCESS:
                var tracker = _trackerRepository.Get(request.TrackerID);
                _ = TrackerConnections.TryGet(request.TrackerID, out var trackerConnection);


                var commandSet = CommandSet.Get(tracker.CommandSet, Program.Host.Services);
                var commandText = GpsWatchMessage.GetCommandText(
                    tracker.Manufacturer,
                    tracker.RawID,
                    commandSet[request.Type],
                    request.Payload
                );
                var commandBytes = Encoding.ASCII.GetBytes(commandText);
                int sentBytes = trackerConnection.Socket.Send(commandBytes);
                if (sentBytes <= 0)
                    throw new Exception("Something wrong happened sending command to the tracker.");


                var trackerReplied = trackerConnection.ResponseQueue.TryTake(out var trackerReplyBase64,
                        _appSettings.SocketOptions.CommandReplyTimeoutMillis);
                var validReply = GpsWatchMessage.TryParse(trackerReplyBase64, out var replyMessage)
                    && replyMessage.ContentItems[0] == commandSet[request.Type];

                if (!trackerReplied || !validReply) {
                    var errorResponse = new CommandResponse {
                        Done = false,
                        Error = ErrorCodes.TRACKER_NO_REPLY
                    };
                    context.Message.Socket.Send(errorResponse.Serialize());
                } else {
                    var response = new CommandResponse { Done = true };
                    context.Message.Socket.Send(response.Serialize());
                }
                #endregion

                return true;

            } catch (Exception ex) {
                var errorResponse = new CommandResponse {
                    Done = false,
                    Error = ErrorCodes.SERVER_ERROR,
                    Payload = ex.Message
                };
                context.Message.Socket.Send(errorResponse.Serialize());
                return false;
            }
        }

        public override bool IsMatch(TrackerMessage message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var bytes = Convert.FromBase64String(message.Base64Text);
            var request = CommandRequest.Deserialize(bytes);
            if (request == null)
                return false;

            return MiddlewareSupportedCommands.Contains(request.Type);
        }

        #region MIDDLEWARE SUPPORTED COMMANDS:
        private static readonly string[] MiddlewareSupportedCommands = new string[] {
            
            CommandSet.COMMAND_UPLOAD_INTERVAL,
            CommandSet.COMMAND_CENTER_NUMBER,
            CommandSet.COMMAND_SECOND_CENTER_NUMBER,
            CommandSet.COMMAND_PASSWORD,
            CommandSet.COMMAND_MAKE_CALL,
            CommandSet.COMMAND_SEND_SMS,
            CommandSet.COMMAND_CALL_CENTER,
            CommandSet.COMMAND_SOS_NUMBERS,
            CommandSet.COMMAND_SOS_FIRST,
            CommandSet.COMMAND_SOS_SECOND,
            CommandSet.COMMAND_SOS_THIRD,
            CommandSet.COMMAND_IP_PORT,
            CommandSet.COMMAND_RESET_FACTORY,
            CommandSet.COMMAND_LANG_ZONE,
            CommandSet.COMMAND_SOS_SMS_ALARM,
            CommandSet.COMMAND_LOWBATTERY_SMS_ALARM,
            CommandSet.COMMAND_APN,
            CommandSet.COMMAND_RESTART,
            CommandSet.COMMAND_WAKEUP,
            CommandSet.COMMAND_POWEROFF,
            CommandSet.COMMAND_REMOVE_ALARM,
            CommandSet.COMMAND_PEDO_FUNC,
            CommandSet.COMMAND_WALK_TIME,
            CommandSet.COMMAND_NO_DISTURBANCE,
            CommandSet.COMMAND_FIND,
            CommandSet.COMMAND_SEND_FLOWERS,
            CommandSet.COMMAND_REMINDER,
            CommandSet.COMMAND_VOICE_MESSAGE,
            CommandSet.COMMAND_TEXT_MESSAGE,
            CommandSet.COMMAND_CONTACTS1,
            CommandSet.COMMAND_CONTACTS2,
            CommandSet.COMMAND_BLUETOOTH,
            CommandSet.COMMAND_SWITCH_SMS,
            CommandSet.COMMAND_SWITCH_AUTO_ANSWER

        };
        #endregion

    }
}
