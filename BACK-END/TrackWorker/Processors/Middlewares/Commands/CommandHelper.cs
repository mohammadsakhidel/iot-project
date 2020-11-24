using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.DataContracts;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Commands {
    public static class CommandHelper {
        public static (bool, string) DoBasicValidation(PipelineContext context, ITrackerRepository trackerRepository, bool payloadRequired = true) {

            var isValid = true;
            var validationError = CommandErrors.INVALID_REQUEST;

            // Deserialize Command:
            var bytes = Convert.FromBase64String(context.Message.Base64Text);
            var request = CommandRequest.Deserialize(bytes);

            // Is Tracker Online:
            var isTrackerOnline = TrackerConnections.IsTrackerOnline(request.TrackerID);
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
    }
}
