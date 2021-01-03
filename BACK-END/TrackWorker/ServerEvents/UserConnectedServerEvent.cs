using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Constants;

namespace TrackWorker.ServerEvents {
    public class UserConnectedServerEvent : ServerEvent {

        public UserConnectedServerEvent(string server) {
            Data = new string[] { server };
        }

        public override string Name => ServerEventNames.USER_CONNECTED;
    }
}
