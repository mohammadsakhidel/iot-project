using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackWorker.Events;
using TrackWorker.Extensions;
using TrackWorker.Models;

namespace TrackWorker.Listeners {
    public abstract class MessageListener : IMessageListener {

        // Fields & Constructors:
        private readonly object _lockForOnDataReceived = new object();
        private readonly object _lockForOnClientDisconnected = new object();

        // Events:
        public event EventHandler<ClientConnectedEventArgs> OnClientConnected;
        public event EventHandler<DataReceivedEventArgs> OnDataReceived;
        public event EventHandler<ClientDisconnectedEventArgs> OnClientDisconnected;

        // Methods:
        public async Task StartListeningAsync(CancellationToken stoppingToken) {
            var self = this;
            var portNumber = GetPortNumber();
            var backlogSize = GetBacklogSize();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ipe = new IPEndPoint(ipAddress, portNumber);

            // Start Listening the Port:
            using var listener = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipe);
            listener.Listen(backlogSize);

            // Start the Background Task:
            await Task.Run(() => {

                while (!stoppingToken.IsCancellationRequested) {
                    // Accept an awaiting connection:
                    var socket = listener.Accept();

                    // Invoke OnClientConnected Event:
                    OnClientConnected?.Invoke(this, new ClientConnectedEventArgs { ClientSocket = socket });

                    // Create A Task for each incomming connection:
                    Task.Run(() => {
                        var buffer = new byte[GetBufferSize()];
                        while (!stoppingToken.IsCancellationRequested) {
                            // Wait for data to receive or connection to disconnect:
                            bool connected = true;
                            int bytesRead = 0;
                            while (bytesRead == 0 && connected) {
                                if (socket.Available > 0) {
                                    bytesRead = socket.Receive(buffer);
                                } else if (!socket.IsConnected()) {
                                    // Invoke OnClietnDisconnected event and exit the task:
                                    lock (_lockForOnClientDisconnected) {
                                        self.OnClientDisconnected?.Invoke(self, new ClientDisconnectedEventArgs {
                                            ClientSocket = socket
                                        });
                                    }
                                    connected = false;
                                }
                            }

                            // End the task if socket not connected anymore
                            if (!connected)
                                break;

                            // Invoke OnDataReceived event:
                            if (bytesRead > 0) {
                                lock (_lockForOnDataReceived) {
                                    self.OnDataReceived?.Invoke(self, new DataReceivedEventArgs {
                                        ClientSocket = socket,
                                        Base64Data = Convert.ToBase64String(buffer, 0, bytesRead)
                                    });
                                }
                            }
                        }
                    });
                }

            });
        }

        // Abstract Members:
        public abstract int GetBufferSize();
        public abstract int GetPortNumber();
        public abstract int GetBacklogSize();
    }
}
