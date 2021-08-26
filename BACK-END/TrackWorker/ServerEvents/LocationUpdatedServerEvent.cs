using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Constants;

namespace TrackWorker.ServerEvents {
    public class LocationUpdatedServerEvent : ServerEvent {

        public LocationUpdatedServerEvent(string trackerId) {
            Source = trackerId;
            Data = new string[] {
                "0.0", // Latitude
                "0.0", // Longitude
                "0.0", // Altitude
                "0.0", // Speed
                "0.0", // Direction
                "0.0"  // Battery
            };
        }

        public override string Name => ServerEventNames.LOCATION_UPDATED;

        public double Latitude {
            set {
                Data[0] = value.ToString();
            }
        }

        public double Longitude {
            set {
                Data[1] = value.ToString();
            }
        }
        public double? Altitude {
            set {
                Data[2] = value.ToString();
            }
        }

        public double? Speed {
            set {
                Data[3] = value.ToString();
            }
        }

        public double? Direction {
            set {
                Data[4] = value.ToString();
            }
        }
        public double? Battery {
            set {
                Data[5] = value.ToString();
            }
        }
    }
}
