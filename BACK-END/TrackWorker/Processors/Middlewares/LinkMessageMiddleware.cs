using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackDataAccess.Repositories;
using TrackLib.Constants;
using TrackLib.Utils;
using TrackWorker.Models;
using TrackWorker.Utils;

namespace TrackWorker.Processors.Middlewares {
    public class LinkMessageMiddleware : Middleware, ILinkMessageMiddleware {

        private readonly ITerminalRepository _terminalRepository;
        public LinkMessageMiddleware(ITerminalRepository terminalRepository) {
            _terminalRepository = terminalRepository;
        }

        public override bool OperateOnMessage(PipelineContext context) {

            #region VALIDATION:
            var baseMessage = context.Message;
            // Validate Message:
            if (baseMessage == null || string.IsNullOrEmpty(baseMessage.Base64Text) || baseMessage.Socket == null)
                return false;

            // Parse Message:
            var message = ThreeGElecMessage.Parse(baseMessage.Base64Text);
            if (message == null)
                return false;

            // Check database for terminal existence:
            var uniqueId = TerminalConnectionUtil.CreateUniqueId(message.Manufacturer.ToLower(), message.TerminalId);
            var terminal = _terminalRepository.Get(uniqueId);
            if (terminal == null)
                return false;
            #endregion

            #region PROCESS MESSAGE:
            // Update terminal last connection fields:
            var publicIP = !string.IsNullOrEmpty(GlobalState.PublicIPAddress) 
                ? GlobalState.PublicIPAddress
                : SocketUtil.FindPublicIPAddressAsync().Result;
            terminal.LastConnection = DateTime.UtcNow;
            terminal.LastConnectedServer = publicIP;
            _terminalRepository.SaveAsync().Wait();

            // Add terminal to connected terminals list:
            TerminalConnectionUtil.Add(uniqueId, baseMessage.Socket.GetRealSocket());

            // Respond to terminal & save to context:
            var response = $"[{message.Manufacturer}*{message.TerminalId}*{MessageAbbreviations.LINK_3G.Length:X4}*{MessageAbbreviations.LINK_3G}]";
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
