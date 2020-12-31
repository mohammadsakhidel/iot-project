using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;

namespace TrackAPI.Models.Polling {
    public class StatusChangedEvent : PollingEvent {
        public StatusChangedEvent(string source, string status, string additionalData = "") {
            Name = PollingEvents.STATUS_CHANGED;
            Source = source;
            Data = new string[] { status, additionalData };
        }
    }
}
