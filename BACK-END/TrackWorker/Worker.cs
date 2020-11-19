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
using TrackWorker.Extensions;
using TrackWorker.Listeners;
using TrackWorker.Models;
using TrackWorker.Processors.LineManagers;

namespace TrackWorker {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IIncomingMessageListener _inMessageListener;
        private readonly IOutgoingMessageListener _outMessageListener;
        private readonly IInLineManager _inLineManager;
        private readonly IOutLineManager _outLineManager;
        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings,
            IIncomingMessageListener inMessageListener, IOutgoingMessageListener outMessageListener,
            IInLineManager inLineManager, IOutLineManager outLineManager) {

            _logger = logger;
            _inMessageListener = inMessageListener;
            _outMessageListener = outMessageListener;
            _inLineManager = inLineManager;
            _outLineManager = outLineManager;

        }

        #region Overrides:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

            try {

                // Listener for messages from terminals:
                var inTask = _inMessageListener.StartListeningAsync(stoppingToken);
                _inMessageListener.OnDataReceived += _inMessageListener_OnDataReceivedAsync;
                _inMessageListener.OnClientDisconnected += _inMessageListener_OnClientDisconnected;

                // Listener for command messager to be sent to the device:
                var outTask = _outMessageListener.StartListeningAsync(stoppingToken);
                _outMessageListener.OnDataReceived += _outMessageListener_OnDataReceivedAsync;

                // Line manager for messages from terminals (IN):
                var inLineTask = _inLineManager.StartWachingAsync(stoppingToken);

                // Line manager for command messages to be sent to terminals:
                var outLineTask = _outLineManager.StartWachingAsync(stoppingToken);

                
                await Task.WhenAll(inTask, outTask, inLineTask, outLineTask);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(ExecuteAsync)));
            }

        }
        #endregion

        #region Event Handlers:
        private async void _inMessageListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var message = new Message {
                    Socket = e.ClientSocket,
                    Base64Text = e.Base64Data,
                    TimeOfCreate = DateTime.UtcNow
                };
                await _inLineManager.AddAsync(message);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(_inMessageListener_OnDataReceivedAsync)));
            }
        }

        private async void _outMessageListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var message = new Message {
                    Socket = e.ClientSocket,
                    Base64Text = e.Base64Data,
                    TimeOfCreate = DateTime.UtcNow
                };
                await _outLineManager.AddAsync(message);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(_outMessageListener_OnDataReceivedAsync)));
            }
        }

        private void _inMessageListener_OnClientDisconnected(object sender, Events.ClientDisconnectedEventArgs e) {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
