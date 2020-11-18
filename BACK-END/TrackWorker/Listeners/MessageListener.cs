using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
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
    public class MessageListener : IMessageListener {

        // Fields & Constructors:
        private readonly object _lockForOnDataReceived = new object();
        private readonly object _lockForOnClientDisconnected = new object();
        private readonly AppSettings _appSettings;
        private readonly byte[] _buffer;

        public MessageListener(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
            _buffer = new byte[_appSettings.SocketOptions.BufferSize];
        }

        // Events:
        public event EventHandler<ClientConnectedEventArgs> OnClientConnected;
        public event EventHandler<DataReceivedEventArgs> OnDataReceived;
        public event EventHandler<ClientDisconnectedEventArgs> OnClientDisconnected;

        // Methods:
        public async Task StartListeningAsync(CancellationToken stoppingToken) {
            var self = this;
            var portNumber = _appSettings.SocketOptions.Messages.PortNumber;
            var backlogSize = _appSettings.SocketOptions.Messages.BacklogSize;
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
                        while (!stoppingToken.IsCancellationRequested) {
                            // Wait for data to receive or connection to disconnect:
                            bool connected = true;
                            int bytesRead = 0;
                            while (bytesRead == 0 && connected) {
                                if (socket.Available > 0) {
                                    bytesRead = socket.Receive(_buffer);
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
                                        Base64Data = Convert.ToBase64String(_buffer, 0, bytesRead)
                                    });
                                }
                            }
                        }
                    });
                }

            });
        }
    }
}
