using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackLib.DataContracts {

    public class CommandResponse {
        public bool Done { get; set; }

        /// <summary>
        /// If succeeded = true the payload will contain needed data based on the request type.
        /// If succedded = false the payload will contain error info.
        /// </summary>
        public string Payload { get; set; }

        public byte[] Serialize() {
            var json = JsonSerializer.Serialize(this);
            return Encoding.ASCII.GetBytes(json);
        }

        public static CommandResponse Deserialize(byte[] bytes) {
            var json = Encoding.ASCII.GetString(bytes);
            return JsonSerializer.Deserialize<CommandResponse>(json);
        }

    }

}
