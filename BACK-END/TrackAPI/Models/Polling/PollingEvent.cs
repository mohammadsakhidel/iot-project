using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models.Polling {
    public abstract class PollingEvent {
        public string Name { get; protected set; }
        public string Source { get; protected set; }
        public string[] Data { get; protected set; }
    }
}
