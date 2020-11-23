using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Extensions;
using TrackWorker.Listeners;
using TrackWorker.Models;
using TrackWorker.Processors.Queues;
using TrackWorker.Utils;

namespace TrackWorker {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IMessageListener _inMessageListener;
        private readonly ICommandListener _outMessageListener;
        private readonly IMessageQueue _inLineManager;
        private readonly ICommandQueue _outLineManager;
        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings,
            IMessageListener inMessageListener, ICommandListener outMessageListener,
            IMessageQueue inLineManager, ICommandQueue outLineManager) {

            _logger = logger;
            _inMessageListener = inMessageListener;
            _outMessageListener = outMessageListener;
            _inLineManager = inLineManager;
            _outLineManager = outLineManager;

        }

        #region Overrides:
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

            try {

                #region Set Global State Values:
                var globalStateTask = Task.Run(() => {
                    var ip = SocketUtil.FindPublicIPAddressAsync(5).Result;
                    GlobalState.SetPublicIPAddress(ip);
                });
                #endregion

                #region Start Listeners:
                // Listener for messages from trackers:
                var inTask = _inMessageListener.StartListeningAsync(stoppingToken);
                _inMessageListener.OnDataReceived += _inMessageListener_OnDataReceivedAsync;
                _inMessageListener.OnClientDisconnected += _inMessageListener_OnClientDisconnected;

                // Listener for command messager to be sent to the device:
                var outTask = _outMessageListener.StartListeningAsync(stoppingToken);
                _outMessageListener.OnDataReceived += _outMessageListener_OnDataReceivedAsync;
                #endregion

                #region Start Line Processors:
                // Line manager for messages from trackers (IN):
                var inLineTask = _inLineManager.StartWachingAsync(stoppingToken);

                // Line manager for command messages to be sent to trackers:
                var outLineTask = _outLineManager.StartWachingAsync(stoppingToken);
                #endregion

                await Task.WhenAll(inTask, outTask, inLineTask, outLineTask, globalStateTask);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(ExecuteAsync)));
            }

        }
        #endregion

        #region Event Handlers:
        private async void _inMessageListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var message = new Message {
                    Socket = new SocketWrapper(e.ClientSocket),
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
                    Socket = new SocketWrapper(e.ClientSocket),
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
