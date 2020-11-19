using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.LineManagers {
    public class InLineManager : LineManager, IInLineManager {

        private readonly IPipeline _pipeline;
        public InLineManager(IInPipeline pipeline) {
            _pipeline = pipeline;
        }

        protected override IPipeline GetPipeline() {
            return _pipeline;
        }
    }
}
