using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Constants {
    public class ClaimNames {

        // JWT Specific:
        public const string ISADMIN = "isadm";
        public const string USER_ID = "usrid";

        // Database Specific:
        public const string GROUP = "group";
        public const string STATE = "state";
        public const string CITY = "city";
        public const string ADDRESS = "address";

        // Both:
        public const string GIVEN_NAME = "gname";
        public const string SURNAME = "sname";
        public const string EMAIL = "email";

    }
}
