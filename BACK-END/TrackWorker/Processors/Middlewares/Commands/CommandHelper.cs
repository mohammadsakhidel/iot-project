using System;
using TrackLib.Constants;
using TrackLib.Commands;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;
using TrackWorker.Services;

namespace TrackWorker.Processors.Middlewares.Commands {
    public static class CommandHelper {
        public static (bool, string) DoBasicValidation(PipelineContext context, ITrackerService trackerService, bool payloadRequired = true) {

            var isValid = true;
            var validationError = ErrorCodes.INVALID_REQUEST;

            // Deserialize Command:
            var bytes = Convert.FromBase64String(context.Message.Base64Text);
            var request = CommandRequest.Deserialize(bytes);

            // Is Tracker Online:
            var isTrackerOnline = TrackerConnections.IsTrackerOnline(request.TrackerID);
            if (!isTrackerOnline) {
                isValid = false;
                validationError = ErrorCodes.TRACKER_OFFLINE;
            }

            // Check database for tracker existence:
            var trackerExists = trackerService.Get(request.TrackerID) != null;
            if (!trackerExists) {
                isValid = false;
                validationError = ErrorCodes.INVALID_REQUEST;
            }

            return (isValid, validationError);

        }
    }
}
