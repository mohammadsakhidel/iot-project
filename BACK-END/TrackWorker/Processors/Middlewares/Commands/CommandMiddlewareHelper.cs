using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.DataContracts;
using TrackLib.Utils;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Utils;

namespace TrackWorker.Processors.Middlewares.Commands {
    public static class CommandMiddlewareHelper {
        public static bool IsMatch(Message message, params string[] validCommandTypes) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var bytes = Convert.FromBase64String(message.Base64Text);
            var commandRequest = CommandRequest.Deserialize(bytes);
            return validCommandTypes.Contains(commandRequest.Type);
        }

        public static (bool, string) DoBasicValidation(PipelineContext context, ITrackerRepository trackerRepository, bool payloadRequired = true) {

            var isValid = true;
            var validationError = CommandErrors.INVALID_REQUEST;

            // Null Inputs check:
            if (context == null || context.Message == null) {
                isValid = false;
                validationError = CommandErrors.INVALID_REQUEST;
            }

            // Deserialize Command:
            var bytes = Convert.FromBase64String(context.Message.Base64Text);
            var commandRequest = CommandRequest.Deserialize(bytes);
            if (commandRequest == null || string.IsNullOrEmpty(commandRequest.TrackerID) || 
                string.IsNullOrEmpty(commandRequest.Type) || (payloadRequired && string.IsNullOrEmpty(commandRequest.Payload))) {
                isValid = false;
                validationError = CommandErrors.INVALID_REQUEST;
            }

            // Is Tracker Online:
            var isTrackerOnline = TrackerConnectionUtil.Exists(commandRequest.TrackerID);
            if (!isTrackerOnline) {
                isValid = false;
                validationError = CommandErrors.TRACKER_OFFLINE;
            }

            // Check database for tracker existence:
            var trackerExists = trackerRepository.Get(commandRequest.TrackerID) != null;
            if (!trackerExists) {
                isValid = false;
                validationError = CommandErrors.INVALID_REQUEST;
            }

            return (isValid, validationError);

        }
    }
}
