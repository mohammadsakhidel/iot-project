using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackWorker.Helpers;

namespace TrackWorker.Processors.Queues {
    public interface IQueue {
        Task AddAsync(Message message);
        Task StartWachingAsync(CancellationToken stoppingToken);
    }
}
