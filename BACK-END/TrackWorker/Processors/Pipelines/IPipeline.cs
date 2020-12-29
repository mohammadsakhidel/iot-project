using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Helpers;

namespace TrackWorker.Processors.Pipelines {
    public interface IPipeline {
        void UseMiddleware<TMiddleWare>();
        Task DispatchAsync(TrackerMessage message, bool onlyValidate = false);
        PipelineContext GetContext();
    }
}
