using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Extensions {
    public static class HttpClientExtensions {
        public static void AddTokenHeader(this HttpClient http, IConfiguration config) {
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {config["API:Token"]}");
        }
    }
}
