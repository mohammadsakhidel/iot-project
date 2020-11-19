using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.LineManagers {
    public class OutLineManager : LineManager, IOutLineManager {

        private readonly IPipeline _pipeline;
        public OutLineManager(IOutPipeline pipeline) {
            _pipeline = pipeline;
        }

        protected override IPipeline GetPipeline() {
            return _pipeline;
        }
    }
}
