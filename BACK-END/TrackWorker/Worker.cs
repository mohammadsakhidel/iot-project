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
        private readonly IMessageListener _messageListener;
        private readonly ICommandListener _commandListener;
        private readonly IMessageQueue _messageQueue;
        private readonly ICommandQueue _commandQueue;
        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings,
            IMessageListener messageListener, ICommandListener commandListener,
            IMessageQueue messageQueue, ICommandQueue commandQueue) {

            _logger = logger;
            _messageListener = messageListener;
            _commandListener = commandListener;
            _messageQueue = messageQueue;
            _commandQueue = commandQueue;

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
                var messageListenerTask = _messageListener.StartListeningAsync(stoppingToken);
                _messageListener.OnDataReceived += MessageListener_OnDataReceivedAsync;
                _messageListener.OnClientDisconnected += MessageListener_OnClientDisconnected;

                // Listener for command messager to be sent to the device:
                var commandListenerTask = _commandListener.StartListeningAsync(stoppingToken);
                _commandListener.OnDataReceived += CommandListener_OnDataReceivedAsync;
                #endregion

                #region Start Queue Listeners:
                // Line manager for messages from trackers (IN):
                var messageQueueListenerTask = _messageQueue.StartWachingAsync(stoppingToken);

                // Line manager for command messages to be sent to trackers:
                var commandQueueListenerTask = _commandQueue.StartWachingAsync(stoppingToken);
                #endregion

                await Task.WhenAll(messageListenerTask, commandListenerTask, 
                    messageQueueListenerTask, commandQueueListenerTask, globalStateTask);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(ExecuteAsync)));
            }

        }
        #endregion

        #region Event Handlers:
        private async void MessageListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var message = new Message {
                    Socket = new SocketWrapper(e.ClientSocket),
                    Base64Text = e.Base64Data,
                    TimeOfCreate = DateTime.UtcNow
                };
                await _messageQueue.AddAsync(message);
                _logger.LogInformation($"MESSAGE RECEIVED -> {Encoding.ASCII.GetString(Convert.FromBase64String(e.Base64Data))}");

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(MessageListener_OnDataReceivedAsync)));
            }
        }

        private async void CommandListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var message = new Message {
                    Socket = new SocketWrapper(e.ClientSocket),
                    Base64Text = e.Base64Data,
                    TimeOfCreate = DateTime.UtcNow
                };
                await _commandQueue.AddAsync(message);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(CommandListener_OnDataReceivedAsync)));
            }
        }

        private void MessageListener_OnClientDisconnected(object sender, Events.ClientDisconnectedEventArgs e) {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
