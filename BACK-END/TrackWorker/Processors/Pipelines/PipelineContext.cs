using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;
using TrackWorker.Processors.Middlewares;

namespace TrackWorker.Processors.Pipelines {
    public class PipelineContext {
        public Message Message { get; set; }
        public bool OnlyValidate { get; set; }

        public bool MessageValid { get; set; }
        public bool MessageProcessed { get; set; }
    }
}
