using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Queues {
    public class CommandQueue : Queue, ICommandQueue {

        private readonly IPipeline _pipeline;
        public CommandQueue(ICommandPipeline pipeline) {
            _pipeline = pipeline;
        }

        protected override IPipeline GetPipeline() {
            return _pipeline;
        }
    }
}
