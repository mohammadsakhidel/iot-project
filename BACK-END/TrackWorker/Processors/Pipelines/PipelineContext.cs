using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Helpers;
using TrackWorker.Processors.Middlewares;

namespace TrackWorker.Processors.Pipelines {
    public class PipelineContext {
        public TrackerMessage Message { get; set; }
        public bool OnlyValidate { get; set; }

        public bool MessageValid { get; set; }
        public bool MessageProcessed { get; set; }
    }
}
