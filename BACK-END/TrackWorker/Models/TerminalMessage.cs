using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Models {
    public abstract class TerminalMessage {
        public Socket Socket { get; set; }
    }
}
