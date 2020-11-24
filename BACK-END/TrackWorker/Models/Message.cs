using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Models {
    public class Message {
        public ISocketWrapper Socket { get; set; }
        public string Base64Text { get; set; }
        public DateTime TimeOfCreate { get; set; }
    }
}
