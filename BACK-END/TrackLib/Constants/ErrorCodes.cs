using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Constants {
    public class ErrorCodes {

        // Command Errors:
        public const string INVALID_REQUEST = "INVALID_REQUEST";
        public const string TRACKER_OFFLINE = "TRACKER_OFFLINE";
        public const string SERVER_ERROR = "SERVER_ERROR";
        public const string INVALID_FORMAT = "INVALID_FORMAT";
        public const string TRACKER_NO_REPLY = "NO_REPLY_FROM_TRACKER";

        // API Errors:
        public const string ALREADY_ADDED = "ALREADY_ADDED";
        public const string NOT_ALLOWED = "NOT_ALLOWED";

    }
}
