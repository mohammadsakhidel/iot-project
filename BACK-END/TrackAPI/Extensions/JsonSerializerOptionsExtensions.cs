using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackAPI.Extensions {
    public static class JsonSerializerOptionsExtensions {
        public static JsonSerializerOptions SetDefaults(this JsonSerializerOptions options) {
            options.IgnoreNullValues = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            return options;
        }
    }
}
