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

                if (OperateOnMessage(context.Message))
                    context.MessageProcessed = true;

            } else {
                Next?.Invoke(context);
            }
        }
        protected abstract bool ValidateMessage(Message message);
        protected abstract bool OperateOnMessage(Message message);
    }
}
