using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Helpers;

namespace TrackWorker.Listeners {
    public class CommandListener : Listener, ICommandListener {

        private readonly AppSettings _appSettings;
        public CommandListener(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public override int GetBacklogSize() {
            return _appSettings.SocketOptions.CommandListener.BacklogSize;
        }

        public override int GetBufferSize() {
            return _appSettings.SocketOptions.BufferSize;
        }

        public override int GetPortNumber() {
            return _appSettings.SocketOptions.CommandListener.PortNumber;
        }
    }
}
