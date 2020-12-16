using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrackLib.Utils {
    public static class PathUtil {
        public static string Resolve(params string[] parts) {
            var regex = new Regex(@"[/\\]");
            for (int i = 0; i < parts.Length; i++) {
                parts[i] = regex.Replace(parts[i], Path.DirectorySeparatorChar.ToString());
            }
            return Path.Combine(parts);
        }
    }
}
