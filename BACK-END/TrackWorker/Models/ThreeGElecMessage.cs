using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;
using TrackLib.Utils;

namespace TrackWorker.Models {
    public class ThreeGElecMessage {
        private ThreeGElecMessage() {

        }

        public string Manufacturer { get; set; }
        public string TerminalId { get; set; }
        public string ContentLengthHex { get; set; }
        public string Content { get; set; }

        // Static Methods:
        public static ThreeGElecMessage Parse(string base64Message) {
            if (!TextUtil.IsBase64String(base64Message))
                return null;

            var bytes = Convert.FromBase64String(base64Message);
            var message = Encoding.ASCII.GetString(bytes);

            var regex = new Regex(Patterns.MESSAGE_LINK);
            if (!regex.IsMatch(message))
                return null;

            var parts = message.Substring(
                    message.IndexOf('[') + 1,
                    message.LastIndexOf(']') - message.IndexOf('[') - 1
                ).Split('*');

            return new ThreeGElecMessage() { 
                Manufacturer = parts[0],
                TerminalId = parts[1],
                ContentLengthHex = parts[2],
                Content = parts[3]
            };
        }
    }
}
