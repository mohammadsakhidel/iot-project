using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Services;

namespace TrackWorker.Processors.Middlewares.Messages {
    public static class MessageHelper {
        public static bool Validate(PipelineContext context, ITrackerService trackerService) {
            // Null Inputs check:
            if (context == null || context.Message == null)
                return false;

            // Parse Message:
            var messageParsed = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            if (!messageParsed)
                return false;

            // Check database for tracker existence:
            var tracker = trackerService.Get(message.UniqueID);
            if (tracker == null || tracker.Id != message.UniqueID)
                return false;

            return true;
        }
    }
}
