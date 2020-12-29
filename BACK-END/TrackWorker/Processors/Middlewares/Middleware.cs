using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Middlewares {
    public abstract class Middleware : IMiddleware {
        public IMiddleware Next { get; set; }
        public void Invoke(PipelineContext context) {
            var isMessageValid = IsMatch(context.Message);
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
        public abstract bool IsMatch(TrackerMessage message);
        public abstract bool OperateOnMessage(PipelineContext context);
    }
}
