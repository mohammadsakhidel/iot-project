using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackAPI.Helpers;
using TrackLib.Constants;

namespace TrackAPI.Middlewares {
    public class JwtAuthenticationMiddleware : IJwtAuthenticationMiddleware {

        private readonly AppSettings _appSettings;
        public JwtAuthenticationMiddleware(IOptions<AppSettings> appSettings) {
            _appSettings = appSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {

            var authHeaderExists = context.Request.Headers.TryGetValue("Authorization", out var authHeader);
            if (authHeaderExists && Regex.IsMatch(authHeader, Patterns.BEARER_JWT_TOKEN)) {
                var tokenText = Regex.Split(authHeader, @"\s+")[1];
                var validToken = tryValidate(tokenText, context, out var principal);
                if (validToken) {
                    context.User = principal;
                }
            }

            await next.Invoke(context);
        }

        private bool tryValidate(string token, HttpContext context, out ClaimsPrincipal principal) {
            try {
                var handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(token, new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT.SecretKey)),
                    ValidIssuer = _appSettings.JWT.Issuer,
                    ValidAudience = _appSettings.JWT.Audience,
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
