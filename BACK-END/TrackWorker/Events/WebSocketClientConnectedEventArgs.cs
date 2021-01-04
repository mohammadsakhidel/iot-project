using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TrackWorker.Helpers;

namespace TrackWorker.Events {
    public class WebSocketClientConnectedEventArgs {
        public WebSocketClient Client { get; set; }
    }
}
