using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.ServerEvents;
using TrackWorker.Services;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class GpsWatchLinkMiddleware : Middleware, IGpsWatchLinkMiddleware {

        private readonly AppSettings _appSettings;
        public GpsWatchLinkMiddleware(IOptions<AppSettings> options) {
            _appSettings = options.Value;
        }

        public override bool OperateOnMessage(PipelineContext context) {

            var trackerService = context.Services.GetService(typeof(ITrackerService)) as ITrackerService;

            #region VALIDATION:
            var isValid = MessageHelper.Validate(context, trackerService);
            if (!isValid)
                return false;
            #endregion

            #region PROCESSING:
            _ = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = trackerService.GetWithIncludeAsync(message.UniqueID).Result;

            // Update tracker last connection fields:
            trackerService.UpdateLastConnectAsync(
                tracker.Id, 
                TrackerStatusValues.ONLINE, 
                _appSettings.ServerName, 
                DateTime.UtcNow
            ).Wait();

            // Add tracker to connected trackers list:
            TrackerConnections.Add(message.UniqueID, new TrackerConnection {
                Socket = context.Message.Socket.GetRealSocket()
            });

            // Send status changed server event to all listening users:
            tracker.Users.ForEach(user => {
                if (UserConnections.Contains(user.UserId)) {
                    var client = UserConnections.Get(user.UserId).Client;
                    var @event = new StatusChangedServerEvent(tracker.Id, TrackerStatusValues.ONLINE);

                    client.Socket.Send(@event.Serialize()).Wait();
                }
            });

            // Respond to tracker & save to context:
            var response = GpsWatchMessage.GetCommandText(message.Manufacturer, message.TrackerId, message.MessageType.Length.ToString("X4"), message.MessageType);
            var responseBytes = Encoding.ASCII.GetBytes(response);
            context.Message.Socket.Send(responseBytes);
            #endregion

            return true;
        }

        public override bool IsMatch(TrackerMessage message) {

            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            return (new Regex(Patterns.MESSAGE_LINK)).IsMatch(text);

        }
    }
}
