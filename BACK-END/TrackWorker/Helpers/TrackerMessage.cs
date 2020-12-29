using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Helpers {
    public class TrackerMessage {
        public ISocketWrapper Socket { get; set; }
        public string Base64Text { get; set; }
        public DateTime TimeOfCreate { get; set; }
    }
}
