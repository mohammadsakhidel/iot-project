using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;

namespace TrackWorker.Utils {
    public static class GlobalState {
        private static readonly object _lock = new object();
        public static string PublicIPAddress { get; private set; }

        public static void SetPublicIPAddress(string ip) {
            if (!string.IsNullOrEmpty(ip) && Regex.IsMatch(ip, Patterns.IP_V4)) {
                lock(_lock) {
                    PublicIPAddress = ip;
                }
            }
        }
    }
}
