using System;
using System.Collections.Generic;
using System.Text;

namespace TrackWorker.Processors {
    public interface IMiddleware {
        void Invoke(PipelineContext context);
    }
}
