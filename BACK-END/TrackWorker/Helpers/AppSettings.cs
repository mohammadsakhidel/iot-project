using System;
using System.Collections.Generic;
using System.Text;

namespace TrackWorker.Helpers {
    public class AppSettings {

        public const string SECTION_NAME = "AppSettings";
        public string ServerName { get; set; }
        public SocketOptions SocketOptions { get; set; }

    }

    public class SocketOptions {
        public int BufferSize { get; set; }
        public ListenerOptions MessageListener { get; set; }
        public ListenerOptions CommandListener { get; set; }
        public ListenerOptions UserListener { get; set; }
        public int CommandReplyTimeoutMillis { get; set; }
        public int UserAccessTokenValidMins { get; set; }
    }

    public class ListenerOptions {
        public int PortNumber { get; set; }
        public int BacklogSize { get; set; }
    }
}
