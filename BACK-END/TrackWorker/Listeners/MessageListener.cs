using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Helpers;

namespace TrackWorker.Listeners {
    public class MessageListener : Listener, IMessageListener {

        private readonly AppSettings _appSettings;
        public MessageListener(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public override int GetBacklogSize() {
            return _appSettings.SocketOptions.MessageListener.BacklogSize;
        }

        public override int GetBufferSize() {
            return _appSettings.SocketOptions.BufferSize;
        }

        public override int GetPortNumber() {
            return _appSettings.SocketOptions.MessageListener.PortNumber;
        }
    }
}
