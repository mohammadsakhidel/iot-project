using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackLib.Commands;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class GpsWatchReplyMiddleware : Middleware, IGpsWatchReplyMiddleware {
        public override bool IsMatch(TrackerMessage message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var parsed = GpsWatchMessage.TryParse(message.Base64Text, out var messageObj);
            if (!parsed)
                return false;

            var messageType = messageObj.ContentItems.FirstOrDefault();
            if (string.IsNullOrEmpty(messageType))
                return false;

            var commandSet = CommandSet.Get(CommandSet.SET_GPSWATCH, Program.Host.Services);
            var isResponseMessage = commandSet.SupportedCommands.Select(c => c.NativeCommand).Contains(messageType);
            return isResponseMessage;
        }

        public override bool OperateOnMessage(PipelineContext context) {

            _ = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            var conFound = TrackerConnections.TryGet(message.UniqueID, out var connection);
            if (conFound) {
                connection.ResponseQueue.Add(context.Message.Base64Text);
            }

            return true;
        }
    }
}
