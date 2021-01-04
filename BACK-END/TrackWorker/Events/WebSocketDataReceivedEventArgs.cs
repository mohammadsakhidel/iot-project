using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TrackWorker.Helpers;

namespace TrackWorker.Events {
    public class WebSocketDataReceivedEventArgs {
        public WebSocketClient Client { get; set; }
        public string Message { get; set; }
    }
}
