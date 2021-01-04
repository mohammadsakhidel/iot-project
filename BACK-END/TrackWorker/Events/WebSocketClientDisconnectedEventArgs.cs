using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TrackWorker.Helpers;

namespace TrackWorker.Events {
    public class WebSocketClientDisconnectedEventArgs {
        public WebSocketClient Client { get; set; }
    }
}
