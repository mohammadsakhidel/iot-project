﻿using System;
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

        protected override void OperateOnMessage(PipelineContext context) {

            var text = Encoding.ASCII.GetString(Convert.FromBase64String(context.Message.Base64Text)).Trim();
            var parts = TextUtil.Split3GElecMessage(text);
            var uniqueId = TerminalConnectionUtil.CreateUniqueId(parts[0], parts[1]);

            // Check database for validity:
            var isValidTerminal = false;
            var terminal = _terminalRepository.Get(uniqueId);
            if (terminal != null) {
                isValidTerminal = true;

                // Update terminal last connection fields:
                var publicIP = SocketUtil.FindPublicIPAddressAsync().Result;
                terminal.LastConnection = DateTime.UtcNow;
                terminal.LastConnectedServer = publicIP;
                
                _terminalRepository.SaveAsync().Wait();
            }
            
            // Return if terminal doesn't exist in database:
            if (!isValidTerminal)
                return;

            // Add terminal to connected terminals list:
            TerminalConnectionUtil.Add(uniqueId, context.Message.Socket);

            // Respond to terminal:
            var response = $"[{parts[0]}*{parts[1]}*{parts[2]}*{parts[3]}]";
            var responseBytes = Encoding.ASCII.GetBytes(response);
            context.Message.Socket.Send(responseBytes);

        }

        protected override bool ValidateMessage(Message message) {
            var text = Encoding.ASCII.GetString(Convert.FromBase64String(message.Base64Text)).Trim();
            return (new Regex(Patterns.MESSAGE_LINK)).IsMatch(text);
        }
    }
}
