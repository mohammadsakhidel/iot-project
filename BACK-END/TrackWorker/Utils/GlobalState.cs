using System;
using System.Collections.Generic;
using System.Text;

namespace TrackWorker.Utils {
    public static class GlobalState {
        public readonly static object Lock = new object();
        public static string PublicIPAddress { get; set; }
    }
}
