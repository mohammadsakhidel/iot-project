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
using TrackWorker.Helpers;
using TrackWorker.Processors.Queues;
using TrackWorker.Shared;
using TrackDataAccess.Repositories;
using TrackWorker.Services;

namespace TrackWorker {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IMessageListener _messageListener;
        private readonly ICommandListener _commandListener;
        private readonly IMessageQueue _messageQueue;
        private readonly ICommandQueue _commandQueue;
        private readonly IUserListener _userListener;
        private readonly AppSettings _appSettings;

        public Worker(ILogger<Worker> logger, IOptions<AppSettings> appSettings,
            IMessageListener messageListener, ICommandListener commandListener,
            IMessageQueue messageQueue, ICommandQueue commandQueue,
            IUserListener userListener, IOptions<AppSettings> options) {

            _logger = logger;
            _messageListener = messageListener;
            _commandListener = commandListener;
            _messageQueue = messageQueue;
            _commandQueue = commandQueue;
            _userListener = userListener;
            _appSettings = options.Value;

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

                // Listener for User connections:
                var userListenerTask = _userListener.StartListeningAsync(stoppingToken);
                _userListener.OnDataReceived += UserListener_OnDataReceivedAsync;
                _userListener.OnClientDisconnected += UserListener_OnClientDisconnected;
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

                var message = new TrackerMessage {
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

        private async void MessageListener_OnClientDisconnected(object sender, Events.ClientDisconnectedEventArgs e) {
            try {
                var trackerRepository = Program.Services.GetService(typeof(ITrackerRepository)) as ITrackerRepository;
                var trackerId = TrackerConnections.FindBySocket(e.ClientSocket);
                if (!string.IsNullOrEmpty(trackerId)) {
                    var tracker = await trackerRepository.GetAsync(trackerId, true);
                    if (tracker != null) {
                        tracker.Status = TrackerStatusValues.OFFLINE;
                        await trackerRepository.SaveAsync();
                    }
                }
            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(MessageListener_OnClientDisconnected)));
            }
        }

        private async void CommandListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var message = new TrackerMessage {
                    Socket = new SocketWrapper(e.ClientSocket),
                    Base64Text = e.Base64Data,
                    TimeOfCreate = DateTime.UtcNow
                };
                await _commandQueue.AddAsync(message);

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(CommandListener_OnDataReceivedAsync)));
            }
        }

        private void UserListener_OnClientDisconnected(object sender, Events.ClientDisconnectedEventArgs e) {
            try {

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(UserListener_OnClientDisconnected)));
            }
        }

        private async void UserListener_OnDataReceivedAsync(object sender, Events.DataReceivedEventArgs e) {
            try {

                var dataBytes = Convert.FromBase64String(e.Base64Data);
                var accessCodeText = Encoding.UTF8.GetString(dataBytes);

                #region Validate Access Token:
                var accessCodeService = Program.Services.GetService(typeof(IAccessCodeService)) as IAccessCodeService;
                var accessCode = await accessCodeService.GetAsync(accessCodeText);

                // Invalid Access Code:
                if (accessCode == null)
                    return;

                // Expired Access Code:
                if (accessCode.CreationTime.AddMinutes(_appSettings.SocketOptions.UserAccessTokenValidMins) < DateTime.UtcNow)
                    return;
                #endregion

                // Add User Connection:
                TrackerConnections.AddUser(accessCode.TrackerId, accessCode.UserId, e.ClientSocket);


            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(UserListener_OnDataReceivedAsync)));
            }
        }
        #endregion
    }
}
