using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;

namespace TrackWorker.Listeners {
    public class IncomingMessageListener : MessageListener, IIncomingMessageListener {

        private readonly AppSettings _appSettings;
        public IncomingMessageListener(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public override int GetBacklogSize() {
            return _appSettings.SocketOptions.IncomingListener.BacklogSize;
        }

        public override int GetBufferSize() {
            return _appSettings.SocketOptions.BufferSize;
        }

        public override int GetPortNumber() {
            return _appSettings.SocketOptions.IncomingListener.PortNumber;
        }
    }
}
