using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using TrackWorker.Services;
using TrackWorker.ServerEvents;

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

                #region Start Listeners:
                // Listener for messages from trackers:
                var messageListenerTask = _messageListener.StartListeningAsync(stoppingToken);
                _messageListener.OnDataReceived += MessageListener_OnDataReceivedAsync;
                _messageListener.OnClientDisconnected += MessageListener_OnClientDisconnected;

                // Listener for command messager to be sent to the device:
                var commandListenerTask = _commandListener.StartListeningAsync(stoppingToken);
                _commandListener.OnDataReceived += CommandListener_OnDataReceivedAsync;

                // Listener for User connections:
                _userListener.StartListening(_appSettings.SocketOptions.UserListener.PortNumber);
                _userListener.OnDataReceived += UserListener_OnDataReceivedAsync;
                #endregion

                #region Start Queue Listeners:
                // Line manager for messages from trackers (IN):
                var messageQueueListenerTask = _messageQueue.StartWachingAsync(stoppingToken);

                // Line manager for command messages to be sent to trackers:
                var commandQueueListenerTask = _commandQueue.StartWachingAsync(stoppingToken);
                #endregion

                await Task.WhenAll(messageListenerTask, commandListenerTask,
                    messageQueueListenerTask, commandQueueListenerTask);

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

                var trackerId = TrackerConnections.FindBySocket(e.ClientSocket);
                if (!string.IsNullOrEmpty(trackerId)) {
                    var trackerService = Program.Services.GetService(typeof(ITrackerService)) as ITrackerService;
                    var tracker = await trackerService.GetWithIncludeAsync(trackerId, true);
                    
                    if (tracker != null) {

                        // Notify connected users:
                        foreach (var user in tracker.Users) {
                            if (UserConnections.Contains(user.UserId)) {
                                var client = UserConnections.Get(user.UserId).Client;
                                var @event = new StatusChangedServerEvent(
                                    tracker.Id, 
                                    TrackerStatusValues.OFFLINE, 
                                    tracker.LastConnection.HasValue ? tracker.LastConnection.Value.ToString(SharedValues.DATETIME_FORMAT) : string.Empty
                                );

                                await client.Socket.Send(@event.Serialize());
                            }
                        }

                        // Save offline status in DB:
                        await trackerService.UpdateStatusAsync(tracker.Id, TrackerStatusValues.OFFLINE);

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

        private async void UserListener_OnDataReceivedAsync(object sender, Events.WebSocketDataReceivedEventArgs e) {
            try {

                var accessCodeText = e.Message;

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
                UserConnections.Add(accessCode.UserId, new UserConnection { 
                    Client = e.Client
                });

                // Return User Connected Server Event:
                var @event = new UserConnectedServerEvent(_appSettings.ServerName);
                await e.Client.Socket.Send(@event.Serialize());

            } catch (Exception ex) {
                _logger.LogError(ex.LogMessage(nameof(UserListener_OnDataReceivedAsync)));
            }
        }
        #endregion
    }
}
