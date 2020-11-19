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
using Microsoft.Extensions.Options;
using TrackWorker.Listeners;
using TrackWorker.Models;

namespace TrackWorker {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IIncomingMessageListener _inMessageListener;
        private readonly IOutgoingMessageListener _outMessageListener;

        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings,
            IIncomingMessageListener inMessageListener, IOutgoingMessageListener outMessageListener) {

            _logger = logger;
            _inMessageListener = inMessageListener;
            _outMessageListener = outMessageListener;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

            try {

                var messageListenerTask = _inMessageListener.StartListeningAsync(stoppingToken);
                _inMessageListener.OnClientConnected += (sender, args) => {
                    try {
                        _logger.LogInformation("Client Connected.");
                    } catch (Exception ex) {
                        _logger.LogError(ex.Message);
                    }
                };
                _inMessageListener.OnDataReceived += (sender, args) => {
                    try {
                        _logger.LogInformation("Data Received: " + Encoding.ASCII.GetString(Convert.FromBase64String(args.Base64Data)));
                    } catch (Exception ex) {
                        _logger.LogError(ex.Message);
                    }
                };
                _inMessageListener.OnClientDisconnected += (sender, args) => {
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
