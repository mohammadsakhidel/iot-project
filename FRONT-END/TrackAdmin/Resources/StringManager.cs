using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Resources {
    public class StringManager {
        public static string Get(string key) {
            var rm = new ResourceManager(typeof(Strings));
            return rm.GetString(key) ?? string.Empty;
        }
    }
}
