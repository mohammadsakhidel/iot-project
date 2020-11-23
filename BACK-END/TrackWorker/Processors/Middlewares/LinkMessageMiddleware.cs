using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Models;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Utils;

namespace TrackWorker.Processors.Middlewares {
    public class LinkMessageMiddleware : Middleware, ILinkMessageMiddleware {

        private readonly ITrackerRepository _trackerRepository;
        public LinkMessageMiddleware(ITrackerRepository trackerRepository) {
            _trackerRepository = trackerRepository;
        }

        public override bool OperateOnMessage(PipelineContext context) {

            #region VALIDATION:
            var baseMessage = context.Message;
            // Validate Message:
            if (baseMessage == null || string.IsNullOrEmpty(baseMessage.Base64Text) || baseMessage.Socket == null)
                return false;

            // Parse Message:
            var messageParsed = ThreeGElecMessage.TryParse(baseMessage.Base64Text, out var message);
            if (!messageParsed)
                return false;

            // Check database for tracker existence:
            var tracker = _trackerRepository.Get(message.UniqueID);
            if (tracker == null)
                return false;
            #endregion

            #region PROCESS MESSAGE:
            // Update tracker last connection fields:
            string publicIP = GlobalState.PublicIPAddress;
            if (string.IsNullOrEmpty(publicIP)) {
                publicIP = SocketUtil.FindPublicIPAddressAsync().Result;
                GlobalState.SetPublicIPAddress(publicIP);
            }
            if (!string.IsNullOrEmpty(publicIP))
                tracker.LastConnectedServer = publicIP;
            tracker.LastConnection = DateTime.UtcNow;
            _trackerRepository.SaveAsync().Wait();

            // Add tracker to connected trackers list:
            TrackerConnectionUtil.Add(message.UniqueID, baseMessage.Socket.GetRealSocket());

            // Respond to tracker & save to context:
            var response = $"[{message.Manufacturer}*{message.TrackerId}*{MessageAbbreviations.LINK_3G.Length:X4}*{MessageAbbreviations.LINK_3G}]";
            var responseBytes = Encoding.ASCII.GetBytes(response);
            baseMessage.Socket.Send(responseBytes);
            context.Response = response;
            #endregion

            return true;
        }

        public override bool ValidateMessage(Message message) {

            if (message == null || string.IsNullOrEmpty(message.Base64Text)
                || !TextUtil.IsBase64String(message.Base64Text))
                return false;

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text));
            return (new Regex(Patterns.MESSAGE_LINK)).IsMatch(text);

        }
    }
}
