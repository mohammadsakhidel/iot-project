using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;

namespace TrackWorker.Processors.Middlewares {
    public class LinkMessageMiddleware : Middleware, ILinkMessageMiddleware {
        protected override void OperateOnMessage(PipelineContext context) {
            var text = Encoding.ASCII.GetString(Convert.FromBase64String(context.Message.Base64Text));
            Console.WriteLine("Processes: " + text);
        }

        protected override bool ValidateMessage(Message message) {
            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            Console.WriteLine("Validated: " + text);
            return text.Contains("LK");
        }
    }
}
