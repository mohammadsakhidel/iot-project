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

namespace TrackWorker.Processors.Middlewares.Commands {
    public class SetIntervalCommandMiddleware : Middleware, ISetIntervalCommandMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        public SetIntervalCommandMiddleware(ITrackerRepository trackerRepository) {
            _trackerRepository = trackerRepository;
        }

        public override bool OperateOnMessage(PipelineContext context) {

            /*
            #region VALIDATION:
            // Null Inputs check:
            if (context == null || context.Message == null)
                return false;

            // Parse Message:
            var messageParsed = ThreeGElecMessage.TryParse(context.Message.Base64Text, out var message);
            if (!messageParsed)
                return false;

            // Check database for tracker existence:
            var tracker = _trackerRepository.Get(message.UniqueID);
            if (tracker == null)
                return false;
            #endregion

            #region PROCESS:

            #endregion
            */

            var bytes = Convert.FromBase64String(context.Message.Base64Text);
            var commandRequest = CommandRequest.Deserialize(bytes);

            var response = new CommandResponse { Done = true, Payload = $"You send us: {commandRequest.Payload}" };
            context.Message.Socket.Send(response.Serialize());

            return true;
        }

        public override bool ValidateMessage(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var bytes = Convert.FromBase64String(message.Base64Text);
            var commandRequest = CommandRequest.Deserialize(bytes);
            return commandRequest.Type == CommandTypes.UPLOAD;
        }
    }
}
