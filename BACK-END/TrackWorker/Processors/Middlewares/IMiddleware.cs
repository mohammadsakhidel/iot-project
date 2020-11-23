using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Middlewares {
    public interface IMiddleware {
        void Invoke(PipelineContext context);
    }
}
