using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackLib.Constants;

namespace TrackAPI.Middlewares {
    public class JwtAuthenticationMiddleware : IJwtAuthenticationMiddleware {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {

            var authHeaderExists = context.Request.Headers.TryGetValue("Authorization", out var authHeader);
            if (authHeaderExists && Regex.IsMatch(authHeader, Patterns.BEARER_JWT_TOKEN)) {
                var tokenText = Regex.Split(authHeader, @"\s+")[1];
                var validToken = TryValidate(tokenText, context, out var principal);
                if (validToken) {
                    context.User = principal;
                }
            }

            await next.Invoke(context);
        }

        private bool TryValidate(string token, HttpContext context, out ClaimsPrincipal principal) {
            try {
                var configuration = (IConfiguration)context.RequestServices.GetService(typeof(IConfiguration));
                var handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(token, new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
            } catch {
                principal = null;
                return false;
            }
            return true;
        }
    }
}
