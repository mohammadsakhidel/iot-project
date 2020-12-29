using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Helpers {
    public class AppSettings {
        public JWTSettings JWT { get; set; }
        public WorkerSettings Worker { get; set; }
        public PollingSettings Polling { get; set; }
    }

    public class JWTSettings {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int SessionTimeout { get; set; }
    }

    public class WorkerSettings {
        public bool UseTrackerLastConnection { get; set; }
        public string DefaultServer { get; set; }
        public int PortNumber { get; set; }
        public int BufferSize { get; set; }
        public int ResponseTimeoutMillis { get; set; }

        public string GetHost(string trackerLastConnectedServer) {
            return UseTrackerLastConnection && !string.IsNullOrEmpty(trackerLastConnectedServer)
                   ? trackerLastConnectedServer
                   : DefaultServer;
        }
    }

    public class PollingSettings {
        public int TimeoutSeconds { get; set; }
        public int StatusCheckDelaySeconds { get; set; }
    }
}