using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Constants;

namespace TrackWorker.ServerEvents {
    public class StatusChangedServerEvent : ServerEvent {

        public StatusChangedServerEvent(string trackerId, string status, string lastConnectionDate = "") {
            Source = trackerId;
            Data = new string[] { status, lastConnectionDate };
        }

        public override string Name => ServerEventNames.STATUS_CHANGED;
    }
}
