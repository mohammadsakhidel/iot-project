using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;

namespace TrackWorker.Processors {
    public abstract class Middleware : IMiddleware {
        public IMiddleware Next { get; set; }
        public void Invoke(PipelineContext context) {
            var isMessageValid = ValidateMessage(context.Message);
            if (isMessageValid) {

                context.MessageValid = true;
                if (context.OnlyValidate)
                    return;

                if (OperateOnMessage(context))
                    context.MessageProcessed = true;

            } else {
                Next?.Invoke(context);
            }
        }
        public abstract bool ValidateMessage(Message message);
        public abstract bool OperateOnMessage(PipelineContext context);
    }
}
