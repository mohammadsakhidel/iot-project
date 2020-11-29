using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Shared {
    public static class Mediator {
        private static readonly Dictionary<string, List<Action<object>>> _dic =
            new Dictionary<string, List<Action<object>>>();

        public static void Subscribe(string token, Action<object> action) {
            if (!_dic.ContainsKey(token)) {
                _dic[token] = new List<Action<object>> { action };
            } else {
                _dic[token].Add(action);
            }
        }

        public static void Unsubscribe(string token, Action<object> action) {
            if (_dic.ContainsKey(token)) {
                _dic[token].Remove(action);
            }
        }

        public static void Notify(string token, object arg = null) {
            if (_dic.ContainsKey(token)) {
                foreach (var action in _dic[token]) {
                    action(arg);
                }
            }
        }

    }
}
