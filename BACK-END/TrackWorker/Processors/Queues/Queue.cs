using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Processors.Queues {
    public abstract class Queue : IQueue {
        private readonly BlockingCollection<TrackerMessage> _queue = new BlockingCollection<TrackerMessage>();

        public async Task AddAsync(TrackerMessage message) {
            var pipeline = GetPipeline();
            await pipeline.DispatchAsync(message, true);
            if (pipeline.GetContext().MessageValid)
                _queue.Add(message);
        }

        public async Task StartWachingAsync(CancellationToken stoppingToken) {
            await Task.Run(() => {
                while (!stoppingToken.IsCancellationRequested) {
                    var message = _queue.Take();
                    GetPipeline().DispatchAsync(message); // I didn't await cause I'm gonna process another message :)
                }
            }, stoppingToken);
        }

        #region Abstract Methods:
        protected abstract IPipeline GetPipeline();
        #endregion
    }
}
