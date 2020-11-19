using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackWorker.Models;

namespace TrackWorker.Processors {
    public interface ILineManager {
        Task AddAsync(Message message);
        Task StartWachingAsync(CancellationToken stoppingToken);
    }
}
