using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Middlewares;

namespace TrackAPI.Extensions {
    public static class IApplicationBuilderExtensions {
        public static void UseJwtAuthentication(this IApplicationBuilder app) {
            app.UseMiddleware<IJwtAuthenticationMiddleware>();            
        }
    }
}
