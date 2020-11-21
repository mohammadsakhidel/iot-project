﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackWorker.Models {
    public class AppSettings {
        public const string SECTION_NAME = "AppSettings";
        public SocketOptions SocketOptions { get; set; }
    }

    public class SocketOptions {
        public int BufferSize { get; set; }
        public ListenerOptions IncomingListener { get; set; }
        public ListenerOptions OutgoingListener { get; set; }
    }

    public class ListenerOptions {
        public int PortNumber { get; set; }
        public int BacklogSize { get; set; }
    }
}