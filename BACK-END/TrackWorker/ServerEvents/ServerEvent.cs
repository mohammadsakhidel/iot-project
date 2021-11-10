using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackWorker.Helpers;

namespace TrackWorker.ServerEvents {
    public abstract class ServerEvent {
        public abstract string Name { get; }
        public string Source { get; set; }
        public string[] Data { get; set; }

        public string Serialize() {
            var jsonString = JsonSerializer.Serialize(this, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return jsonString;
        }

        public static void SendToAll(ServerEvent @event, List<WebSocketClient> clients) {

            var tasks = new List<Task>();
            clients.ForEach(client => {
                tasks.Add(client.Socket.Send(@event.Serialize()));
            });
            Task.WaitAll(tasks.ToArray());

        }

    }
}
