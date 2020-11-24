using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.DataContracts;
using TrackLib.Utils;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Utils;

namespace TrackWorker.Processors.Middlewares.Commands {
    public class SetValueCommandMiddleware : Middleware, ISendValueCommandMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        public SetValueCommandMiddleware(ITrackerRepository trackerRepository) {
            _trackerRepository = trackerRepository;
        }

        public override bool OperateOnMessage(PipelineContext context) {
            try {

                #region VALIDATION:
                // Do Basic Validation:
                (var isValid, var validationError) = this.doValidation(context, _trackerRepository);
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
                var tracker = _trackerRepository.Get(request.TrackerID);
                TrackerConnectionUtil.TryGet(request.TrackerID, out var trackerSocket);

                // Send command to the tracher:
                var commandText = ThreeGElecMessage.GetCommandText(tracker.Manufacturer,
                    tracker.RawID, request.Type, request.Payload);
                var commandBytes = Encoding.ASCII.GetBytes(commandText);
                int sentBytes = trackerSocket.Send(commandBytes);
                if (sentBytes > 0) {
                    var response = new CommandResponse { Done = true };
                    context.Message.Socket.Send(response.Serialize());
                } else {
                    throw new Exception("Something wrong happened sending command to the tracker.");
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
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var bytes = Convert.FromBase64String(message.Base64Text);
            var request = CommandRequest.Deserialize(bytes);
            if (request == null)
                return false;

            return CommandTypes.AllSendValueCommands().Contains(request.Type);
        }

        #region Private Methods:
        public (bool, string) doValidation(PipelineContext context, ITrackerRepository trackerRepository, bool payloadRequired = true) {

            var isValid = true;
            var validationError = CommandErrors.INVALID_REQUEST;

            // Null Inputs check:
            if (context == null || context.Message == null) {
                isValid = false;
                validationError = CommandErrors.INVALID_REQUEST;
            }

            // Deserialize Command:
            var bytes = Convert.FromBase64String(context.Message.Base64Text);
            var request = CommandRequest.Deserialize(bytes);
            if (request == null || string.IsNullOrEmpty(request.TrackerID) || string.IsNullOrEmpty(request.Type) || 
                (payloadRequired && (string.IsNullOrEmpty(request.Payload) || !Regex.IsMatch(request.Payload, Patterns.COMMAND_PAYLOAD)))) {
                isValid = false;
                validationError = CommandErrors.INVALID_REQUEST;
            }

            // Is Tracker Online:
            var isTrackerOnline = TrackerConnectionUtil.Exists(request.TrackerID);
            if (!isTrackerOnline) {
                isValid = false;
                validationError = CommandErrors.TRACKER_OFFLINE;
            }

            // Check database for tracker existence:
            var trackerExists = trackerRepository.Get(request.TrackerID) != null;
            if (!trackerExists) {
                isValid = false;
                validationError = CommandErrors.INVALID_REQUEST;
            }

            return (isValid, validationError);

        }
        #endregion

    }
}
