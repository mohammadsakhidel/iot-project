using System;
using System.Collections.Generic;
using System.Text;

namespace TrackLib.Utils {
    public static class TextUtil {
        public static bool IsBase64String(string base64) {
            if (string.IsNullOrEmpty(base64))
                return false;

            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}
