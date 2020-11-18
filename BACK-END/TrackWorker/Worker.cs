using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackWorker.Listeners;

namespace TrackWorker {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IListener _messageListener;

        public Worker(ILogger<Worker> logger,
            IMessageListener messageListener) {

            _logger = logger;
            _messageListener = messageListener;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

            try {

                var messageListenerTask = _messageListener.StartListeningAsync(stoppingToken);
                _messageListener.OnClientConnected += (sender, args) => {
                    try {
                        _logger.LogInformation("Client Connected.");
                    } catch (Exception ex) {
                        _logger.LogError(ex.Message);
                    }
                };
                _messageListener.OnDataReceived += (sender, args) => {
                    try {
                        _logger.LogInformation("Data Received: " + Encoding.ASCII.GetString(Convert.FromBase64String(args.Base64Data)));
                    } catch (Exception ex) {
                        _logger.LogError(ex.Message);
                    }
                };
                _messageListener.OnClientDisconnected += (sender, args) => {
                    try {
                        _logger.LogInformation("Client Disconnected.");
                    } catch (Exception ex) {
                        _logger.LogError(ex.Message);
                    }
                };

                await Task.WhenAll(messageListenerTask);

            } catch (Exception ex) {
                _logger.LogError(ex.Message);
            }

        }
    }
}
