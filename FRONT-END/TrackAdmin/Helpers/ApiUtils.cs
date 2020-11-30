using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Helpers {
    public static class ApiUtils {
        public static Uri Combine(Uri baseUri, Uri tail) {
            return new Uri(baseUri, tail);
        }
        public static Uri Combine(string baseUri, string tail) {
            var bas = new Uri(baseUri);
            return new Uri(bas, tail);
        }
    }
}
