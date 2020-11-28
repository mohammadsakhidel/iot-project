using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Helpers {
    public class AppSettings {
        public JWTSettings JWT { get; set; }
    }

    public class JWTSettings {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int SessionTimeout { get; set; }
    }
}
