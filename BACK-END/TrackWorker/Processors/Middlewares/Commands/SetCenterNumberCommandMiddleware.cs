using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.DataContracts;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Utils;

namespace TrackWorker.Processors.Middlewares.Commands {
    public class SetCenterNumberCommandMiddleware : Middleware, ISetCenterNumberCommandMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        public SetCenterNumberCommandMiddleware(ITrackerRepository trackerRepository) {
            _trackerRepository = trackerRepository;
        }

        public override bool OperateOnMessage(PipelineContext context) {
            try {

                #region VALIDATION:
                // Do Basic Validation:
                (var isValid, var validationError) = CommandMiddlewareHelper.DoBasicValidation(context, _trackerRepository);
                var request = CommandRequest.Deserialize(Convert.FromBase64String(context.Message.Base64Text));

                // Validate Arguments:
                var centerNumber = request.Payload;
                if (!Regex.IsMatch(centerNumber, Patterns.TRACKER_PHONE_NUMBER)) {
                    isValid = false;
                    validationError = CommandErrors.INVALID_FORMAT;
                }

                // Return error response if not valid:
                if (!isValid) {
                    var errorResponse = new CommandResponse { Done = false, Error = validationError };
                    context.Message.Socket.Send(errorResponse.Serialize());
                    return false;
                }
                #endregion

                #region PROCESS:
                var tracker = _trackerRepository.Get(request.TrackerID);
                TrackerConnectionUtil.TryGet(request.TrackerID, out var trackerSocket);

                // Send command to the tracher:
                var commandText = ThreeGElecMessage.CreateString(tracker.Manufacturer,
                    tracker.RawID, CommandTypes.CENTER, centerNumber);
                var commandBytes = Encoding.ASCII.GetBytes(commandText);
                int sentBytes = trackerSocket.Send(commandBytes);
                if (sentBytes > 0) {
                    var response = new CommandResponse { Done = true };
                    context.Message.Socket.Send(response.Serialize());
                }
                #endregion

                return true;

            } catch (Exception ex) {
                var errorResponse = new CommandResponse {
                    Done = false,
                    Error = CommandErrors.SERVER_ERROR,
                    Payload = ex.Message
                };
                context.Message.Socket.Send(errorResponse.Serialize());
                return false;
            }
        }

        public override bool IsMatch(Message message) {
            return CommandMiddlewareHelper.IsMatch(message, CommandTypes.CENTER);
        }
    }
}
