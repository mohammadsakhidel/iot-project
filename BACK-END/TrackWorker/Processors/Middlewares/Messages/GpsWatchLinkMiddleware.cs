using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Helpers;
using TrackWorker.Processors.Pipelines;
using TrackWorker.ServerEvents;
using TrackWorker.Shared;

namespace TrackWorker.Processors.Middlewares.Messages {
    public class GpsWatchLinkMiddleware : Middleware, IGpsWatchLinkMiddleware {

        public override bool OperateOnMessage(PipelineContext context) {

            var trackerRepository = context.Services.GetService(typeof(ITrackerRepository)) as ITrackerRepository;

            #region VALIDATION:
            var isValid = MessageHelper.Validate(context, trackerRepository);
            if (!isValid)
                return false;
            #endregion

            #region PROCESSING:
            _ = GpsWatchMessage.TryParse(context.Message.Base64Text, out var message);
            var tracker = trackerRepository.GetWithIncludeAsync(message.UniqueID).Result;

            // Update tracker last connection fields:
            string publicIP = GlobalState.PublicIPAddress;
            if (string.IsNullOrEmpty(publicIP)) {
                publicIP = SocketUtil.FindPublicIPAddressAsync().Result;
                GlobalState.SetPublicIPAddress(publicIP);
            }
            tracker.Status = TrackerStatusValues.ONLINE;
            if (!string.IsNullOrEmpty(publicIP))
                tracker.LastConnectedServer = publicIP;
            tracker.LastConnection = DateTime.UtcNow;
            trackerRepository.SaveAsync().Wait();

            // Add tracker to connected trackers list:
            TrackerConnections.Add(message.UniqueID, new TrackerConnection {
                Socket = context.Message.Socket.GetRealSocket()
            });

            // Send status changed server event to all listening users:
            tracker.Users.ForEach(user => {
                if (UserConnections.Contains(user.UserId)) {
                    var socket = UserConnections.Get(user.UserId).Socket;
                    var @event = new StatusChangedServerEvent(tracker.Id, TrackerStatusValues.ONLINE);

                    @event.Send(socket);
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
