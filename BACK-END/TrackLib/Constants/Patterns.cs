using System;
using System.Collections.Generic;
using System.Text;

namespace TrackLib.Constants {
    public class Patterns {
        public const string MESSAGE_3GELEC = @"^\[[a-zA-Z0-9]{2}\*[a-zA-Z0-9]+\*[a-fA-F0-9]{4}\*[a-zA-Z]{2}[^*]*\]$";
        public const string MESSAGE_LINK = @"^\[[a-zA-Z0-9]{2}\*[a-zA-Z0-9]+\*[a-fA-F0-9]{4}\*LK[^*]*\]$";
        public const string MESSAGE_LOCATION = @"^\[[a-zA-Z0-9]{2}\*[a-zA-Z0-9]+\*[a-fA-F0-9]{4}\*UD(,[^*]+)+\]$";
        public const string MESSAGE_ALARM = @"^\[[a-zA-Z0-9]{2}\*[a-zA-Z0-9]+\*[a-fA-F0-9]{4}\*AL(,[^*]+)+\]$";

        public const string COMMAND_PAYLOAD = @"^([^,]+)(,[^,]+)*$";

        public const string IP_V4 = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        public const string HEX_NUMBER = @"^[0-9a-fA-F]+$";
        public const string TRACKER_PHONE_NUMBER = @"^\+?\d{10,14}$";
    }
}
