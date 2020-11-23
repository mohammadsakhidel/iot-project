using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Queues {
    public class MessageQueue : Queue, IMessageQueue {

        private readonly IPipeline _pipeline;
        public MessageQueue(IMessagePipeline pipeline) {
            _pipeline = pipeline;
        }

        protected override IPipeline GetPipeline() {
            return _pipeline;
        }
    }
}
