using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackLib.Utils {
    public static class SocketUtil {
        public static async Task<string> FindPublicIPAddressAsync(int tries = 2) {
			try {
                var address = "https://api.ipify.org/?format=text";
                using var httpClient = new HttpClient();
                var t = 0;
                while (t < tries) {
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                        return (await response.Content.ReadAsStringAsync()).Trim();
                    else {
                        t++;
                        Task.Delay(1000).Wait();
                    }
                }
                return "";
			} catch {
                return "";
			}
        }
    }
}
