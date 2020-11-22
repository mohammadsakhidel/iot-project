using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<string> ContentItems { get; set; }
        public string UniqueID => $"{Manufacturer}-{TerminalId}";

        // Static Methods:
        public static bool TryParse(string base64Message, out ThreeGElecMessage parsedMessage) {
            parsedMessage = null;
            if (!TextUtil.IsBase64String(base64Message))
                return false;

            var bytes = Convert.FromBase64String(base64Message);
            var message = Encoding.ASCII.GetString(bytes).ToUpper();

            var regex = new Regex(Patterns.MESSAGE_3GELEC);
            if (!regex.IsMatch(message))
                return false;

            var parts = message.Substring(
                    message.IndexOf('[') + 1,
                    message.LastIndexOf(']') - message.IndexOf('[') - 1
                ).Split('*');

            var contentSeperator = new Regex(@"\s*,\s*");

            parsedMessage = new ThreeGElecMessage() { 
                Manufacturer = parts[0],
                TerminalId = parts[1],
                ContentLengthHex = parts[2],
                Content = parts[3],
                ContentItems = contentSeperator.Split(parts[3]).ToList()
            };
            return true;
        }
    }
}
