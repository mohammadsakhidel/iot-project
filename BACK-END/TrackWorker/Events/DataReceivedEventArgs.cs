using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Events {
    public class DataReceivedEventArgs {
        public Socket ClientSocket { get; set; }
        public string Base64Data { get; set; }
    }
}
