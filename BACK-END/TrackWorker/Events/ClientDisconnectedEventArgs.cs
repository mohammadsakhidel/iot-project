﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Events {
    public class ClientDisconnectedEventArgs {
        public Socket ClientSocket { get; set; }
    }
}
