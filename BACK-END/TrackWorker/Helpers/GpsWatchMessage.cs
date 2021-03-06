using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;
using TrackLib.Utils;

namespace TrackWorker.Helpers {
    public class GpsWatchMessage {
        private GpsWatchMessage() {

        }

        public string Manufacturer { get; set; }
        public string TrackerId { get; set; }
        public string ContentLengthHex { get; set; }
        public string Content { get; set; }
        public List<string> ContentItems { get; set; }
        public string UniqueID => $"{Manufacturer}-{TrackerId}";
        public string MessageType => ContentItems.FirstOrDefault();
        public string MessagePayload {
            get {
                if (ContentItems.Count <= 1)
                    return "";

                return Content[(Content.IndexOf(',') + 1)..Content.Length];
            }
        }

        // Static Methods:
        public static bool TryParse(string base64Message, out GpsWatchMessage parsedMessage) {
            parsedMessage = null;
            if (!TextUtil.IsBase64String(base64Message))
                return false;

            var bytes = Convert.FromBase64String(base64Message);
            var message = Encoding.ASCII.GetString(bytes).ToUpper();

            if (!Regex.IsMatch(message, Patterns.MESSAGE_3GELEC))
                return false;

            var parts = message.Substring(
                    message.IndexOf('[') + 1,
                    message.LastIndexOf(']') - message.IndexOf('[') - 1
                ).Split('*');

            var contentSeperator = new Regex(@"\s*,\s*");

            parsedMessage = new GpsWatchMessage() {
                Manufacturer = parts[0],
                TrackerId = parts[1],
                ContentLengthHex = parts[2],
                Content = parts[3],
                ContentItems = contentSeperator.Split(parts[3]).ToList()
            };
            return true;
        }
        public static string GetCommandText(string manufacturer, string trackerId, string command, params object[] args) {

            string data = args != null && args.Any() ? args.Aggregate((a, b) => $"{a},{b}").ToString() : "";
            string content = command + (string.IsNullOrEmpty(data) ? "" : $",{data}");
            int dataLength = content.Length;

            return string.Format("[{0}*{1}*{2}*{3}]",
                    manufacturer, trackerId, dataLength.ToString("X").PadLeft(4, '0'), content);
        }
    }
}
