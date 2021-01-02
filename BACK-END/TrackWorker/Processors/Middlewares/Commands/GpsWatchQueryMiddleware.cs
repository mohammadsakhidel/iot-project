using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using TrackLib.Constants;
using TrackLib.Commands;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;
using TrackWorker.Services;

namespace TrackWorker.Processors.Middlewares.Commands {
    public class GpsWatchQueryMiddleware : Middleware, IGpsWatchQueryMiddleware {

        private readonly AppSettings _appSettings;
        public GpsWatchQueryMiddleware(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
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

        public override bool OperateOnMessage(PipelineContext context) {
            try {

                var trackerService = context.Services.GetService(typeof(ITrackerService)) as ITrackerService;

                #region VALIDATION:
                // Do Basic Validation:
                (var isValid, var validationError) = CommandHelper.DoBasicValidation(context, trackerService);
                var requestBytes = Convert.FromBase64String(context.Message.Base64Text);
                var request = CommandRequest.Deserialize(requestBytes);

                // Return error response if not valid:
                if (!isValid) {
                    var errorResponse = new CommandResponse { Done = false, Error = validationError };
                    context.Message.Socket.Send(errorResponse.Serialize());
                    return false;
                }
                #endregion

                #region PROCESS:
                var tracker = trackerService.Get(request.TrackerID);
                _ = TrackerConnections.TryGet(request.TrackerID, out var trackerConnection);

                // Send command to the tracker:
                var commandSet = CommandSet.Get(tracker.CommandSet, Program.Services);
                var commandText = GpsWatchMessage.GetCommandText(
                    tracker.Manufacturer,
                    tracker.RawID,
                    commandSet[request.Type],
                    string.Empty
                );
                var commandBytes = Encoding.ASCII.GetBytes(commandText);
                int sentBytes = trackerConnection.Socket.Send(commandBytes);
                if (sentBytes <= 0)
                    throw new Exception("Something wrong happened sending command to the tracker.");

                // Wait for the tracker response:
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
                    var response = new CommandResponse { Done = true, Payload = replyMessage.MessagePayload };
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

        #region MIDDLEWARE SUPPORTED COMMANDS:
        private static readonly string[] MiddlewareSupportedCommands = new string[] {
            CommandSet.COMMAND_GET_STATUS,
            CommandSet.COMMAND_GET_VERSION,
            CommandSet.COMMAND_GET_CURRENT_SETTINGS
        };
        #endregion
    }
}
