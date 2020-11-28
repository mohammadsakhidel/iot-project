using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Constants {
    public class CommandSetNames {
        public const string DEFAULT = "default";

        public static List<string> All() {
            return new List<string> { 
                DEFAULT
            };
        }

    }
}
