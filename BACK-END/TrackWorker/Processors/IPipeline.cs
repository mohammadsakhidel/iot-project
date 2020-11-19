using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Models;

namespace TrackWorker.Processors {
    public interface IPipeline {
        void UseMiddleware<TMiddleWare>();
        Task DispatchAsync(Message message, bool onlyValidate = false);
        PipelineContext GetContext();
    }
}
