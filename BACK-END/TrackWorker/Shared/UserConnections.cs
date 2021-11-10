using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Helpers;

namespace TrackWorker.Shared {
    public static class UserConnections {
        private static object _lock = new object();
        private static Dictionary<string, List<UserConnection>> _users = new Dictionary<string, List<UserConnection>>();

        public static void Add(string userId, UserConnection connection) {
            lock (_lock) {
                if (!_users.ContainsKey(userId))
                    _users.Add(userId, new List<UserConnection> { connection });
                else {
                    if (!_users[userId].Any(c => c.Client.Socket == connection.Client.Socket))
                        _users[userId].Add(connection);
                }
            }
        }

        public static bool Contains(string userId) {
            return _users.ContainsKey(userId);
        }

        public static List<UserConnection> Get(string userId) {
            return _users.GetValueOrDefault(userId);
        }

    }
}
