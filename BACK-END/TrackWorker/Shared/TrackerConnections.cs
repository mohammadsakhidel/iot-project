using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TrackWorker.Extensions;
using TrackWorker.Helpers;

namespace TrackWorker.Shared {
    public static class TrackerConnections {

        private static object _lock = new object();
        private static Dictionary<string, TrackerConnection> _trackers = new Dictionary<string, TrackerConnection>();

        public static void Add(string uniqueId, TrackerConnection connection) {
            lock (_lock) {
                if (!_trackers.ContainsKey(uniqueId))
                    _trackers.Add(uniqueId, connection);
                else {
                    if (_trackers[uniqueId].Socket != connection.Socket)
                        _trackers[uniqueId] = connection;
                }
            }
        }

        public static bool TryGet(string uniqueId, out TrackerConnection connection) {
            if (!IsTrackerOnline(uniqueId)) {
                connection = null;
                return false;
            }
            connection = _trackers[uniqueId];
            return true;
        }

        public static bool IsTrackerOnline(string uniqueId) {
            var exists = _trackers.ContainsKey(uniqueId);
            if (!exists)
                return false;

            return _trackers[uniqueId].Socket.IsConnected();
        }

        public static bool Exists(string uniqueId) {
            return _trackers.ContainsKey(uniqueId);
        }
    }
}
