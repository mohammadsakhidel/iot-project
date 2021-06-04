using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Constants {
    public class CommandSetNames {
        public const string GPSWATCH = "gpswatch";

        public static List<string> All() {
            return new List<string> {
                GPSWATCH
            };
        }
    }
}
