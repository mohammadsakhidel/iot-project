using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackWorker.Events;
using TrackWorker.Helpers;

namespace TrackWorker.Listeners {
    public class UserListener : Listener, IUserListener {

        private readonly AppSettings _appSettings;
        public UserListener(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public override int GetBacklogSize() {
            return _appSettings.SocketOptions.UserListener.BacklogSize;
        }

        public override int GetBufferSize() {
            return _appSettings.SocketOptions.BufferSize;
        }

        public override int GetPortNumber() {
            return _appSettings.SocketOptions.UserListener.PortNumber;
        }
    }
}
