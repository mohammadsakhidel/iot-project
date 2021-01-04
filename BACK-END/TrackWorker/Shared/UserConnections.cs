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
        private static Dictionary<string, UserConnection> _users = new Dictionary<string, UserConnection>();

        public static void Add(string userId, UserConnection connection) {
            lock (_lock) {
                if (!_users.ContainsKey(userId))
                    _users.Add(userId, connection);
                else {
                    if (_users[userId].Client.Socket != connection.Client.Socket)
                        _users[userId] = connection;
                }
            }
        }

        public static bool Contains(string userId) {
            return _users.ContainsKey(userId);
        }

        public static UserConnection Get(string userId) {
            return _users.GetValueOrDefault(userId);
        }

    }
}
