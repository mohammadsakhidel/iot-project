using System;
using System.Collections.Generic;
using System.Text;

namespace TrackLib.Constants {
    public class Patterns {
        public const string MESSAGE_LINK = @"^\[[a-zA-Z0-9]+\*[a-zA-Z0-9]+\*[a-fA-F0-9]+\*LK[^*]*\]$";
        public const string IP_V4 = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
    }
}
