using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Utils {
    public static class TerminalConnectionUtil {

        private static object _lock = new object();
        private static Dictionary<string, Socket> _terminals = new Dictionary<string, Socket>();

        public static string CreateUniqueId(string manufacturer, string terminalId) {
            return $"{manufacturer}-{terminalId}".ToLower();
        }

        public static void Add(string uniqueId, Socket socket) {
            lock (_lock) {
                if (!_terminals.ContainsKey(uniqueId))
                    _terminals.Add(uniqueId, socket);
                else
                    _terminals[uniqueId] = socket;
            }
        }
    }
}
