using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Middlewares.Messages {
    public static class MessageHelper {
        public static bool Validate(PipelineContext context, ITrackerRepository trackerRepository) {
            // Null Inputs check:
            if (context == null || context.Message == null)
                return false;

            // Parse Message:
            var messageParsed = ThreeGElecMessage.TryParse(context.Message.Base64Text, out var message);
            if (!messageParsed)
                return false;

            // Check database for tracker existence:
            var tracker = trackerRepository.Get(message.UniqueID);
            if (tracker == null || tracker.Id != message.UniqueID)
                return false;

            return true;
        }
    }
}
