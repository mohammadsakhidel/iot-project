using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Utils {
    public static class TrackerConnectionUtil {

        private static object _lock = new object();
        private static Dictionary<string, Socket> _trackers = new Dictionary<string, Socket>();

        public static void Add(string uniqueId, Socket socket) {
            lock (_lock) {
                if (!_trackers.ContainsKey(uniqueId))
                    _trackers.Add(uniqueId, socket);
                else
                    _trackers[uniqueId] = socket;
            }
        }

        public static bool Exists(string uniqueId) {
            return _trackers.ContainsKey(uniqueId);
        }
    }
}
