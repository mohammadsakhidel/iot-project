using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Utils {
    public static class SocketUtil {
        public static async Task<string> FindPublicIPAddressAsync(int tries = 2) {
			try {
                var address = "http://icanhazip.com/";
                using var httpClient = new HttpClient();
                var t = 0;
                while (t < tries) {
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsStringAsync();
                    else t++;
                }
                return "";
			} catch {
                return "";
			}
        }
    }
}
