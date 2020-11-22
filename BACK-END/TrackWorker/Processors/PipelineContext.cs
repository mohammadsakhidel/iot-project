using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;

namespace TrackWorker.Processors {
    public class PipelineContext {
        public Message Message { get; set; }
        public bool OnlyValidate { get; set; }

        public bool MessageValid { get; set; }
        public bool MessageProcessed { get; set; }

        public IMiddleware AssociatedMiddleware { get; set; }
        public string Response { get; set; }
    }
}
