using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackLib.Commands {

    public class CommandRequest {

        public CommandRequest() {

        }

        public CommandRequest(string trackerId, string type)
            : this(trackerId, type, string.Empty) {
        }

        public CommandRequest(string trackerId, string type, string payload) {
            TrackerID = trackerId;
            Type = type;
            Payload = payload;
        }

        public string Type { get; set; }
        public string TrackerID { get; set; }
        public string Payload { get; set; }

        public byte[] Serialize() {
            var json = JsonSerializer.Serialize(this);
            return Encoding.ASCII.GetBytes(json);
        }

        public static CommandRequest Deserialize(byte[] bytes) {
            try {
                var json = Encoding.ASCII.GetString(bytes);
                return JsonSerializer.Deserialize<CommandRequest>(json);
            } catch {
                return null;
            }
        }

    }

}
