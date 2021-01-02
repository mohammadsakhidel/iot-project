using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackWorker.ServerEvents {
    public abstract class ServerEvent {
        public abstract string Name { get; }
        public string Source { get; set; }
        public string[] Data { get; set; }

        public bool Send(Socket socket) {
            try {

                var jsonString = JsonSerializer.Serialize(this, new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                socket.Send(bytes);

                return true;

            } catch {
                return false;
            }
        }

    }
}
