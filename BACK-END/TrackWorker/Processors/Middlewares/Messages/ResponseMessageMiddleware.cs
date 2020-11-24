using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class ResponseMessageMiddleware : Middleware, IResponseMessageMiddleware {
        public override bool IsMatch(Message message) {
            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var parsed = ThreeGElecMessage.TryParse(message.Base64Text, out var messageObj);
            var messageType = messageObj.ContentItems.FirstOrDefault();
            if (string.IsNullOrEmpty(messageType))
                return false;

            return CommandTypes.AllSendValueCommands().Contains(messageType);
        }

        public override bool OperateOnMessage(PipelineContext context) {

            _ = ThreeGElecMessage.TryParse(context.Message.Base64Text, out var message);
            var conFound = TrackerConnections.TryGet(message.UniqueID, out var connection);
            if (conFound) {
                connection.ResponseQueue.Add(context.Message.Base64Text);
            }

            return true;
        }
    }
}
