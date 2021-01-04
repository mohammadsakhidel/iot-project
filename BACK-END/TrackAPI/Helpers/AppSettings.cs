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
        public int BufferSize { get; set; }
        public int ResponseTimeoutMillis { get; set; }
        public WorkerServerInfo[] Servers { get; set; }

        public WorkerServerInfo GetHost(string trackerLastConnectedServer) {
            var lastConnectedServer = UseTrackerLastConnection && !string.IsNullOrEmpty(trackerLastConnectedServer)
                   ? Servers.SingleOrDefault(s => s.Name == trackerLastConnectedServer.ToUpper())
                   : null;

            return lastConnectedServer ?? Servers.First();
        }
    }

    public class PollingSettings {
        public int TimeoutSeconds { get; set; }
        public int StatusCheckDelaySeconds { get; set; }
    }

    public class WorkerServerInfo {
        public string Name { get; set; }
        public string IP { get; set; }
        public int CommandPort { get; set; }
        public int UserPort { get; set; }
    }
}