using System;
using System.Collections.Generic;
using System.Text;

namespace TrackLib.Utils {
    public static class TextUtil {
        public static string[] Split3GElecMessage(string text) {
            return text.Substring(
                    text.IndexOf('[') + 1,
                    text.LastIndexOf(']') - text.IndexOf('[') - 1
                ).Split('*');
        }
    }
}
