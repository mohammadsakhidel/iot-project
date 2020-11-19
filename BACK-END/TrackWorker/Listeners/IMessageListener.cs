using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackWorker.Events;
using TrackWorker.Models;

namespace TrackWorker.Listeners {
    public interface IMessageListener {

        // Events:
        event EventHandler<ClientConnectedEventArgs> OnClientConnected;
        event EventHandler<DataReceivedEventArgs> OnDataReceived;
        event EventHandler<ClientDisconnectedEventArgs> OnClientDisconnected;

        // Methods:
        Task StartListeningAsync(CancellationToken stoppingToken);

    }
}
