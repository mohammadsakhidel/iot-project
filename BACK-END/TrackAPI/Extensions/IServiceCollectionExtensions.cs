using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Helpers;
using TrackDataAccess.Database;

namespace TrackAPI.Extensions {
    public static class IServiceCollectionExtensions {

        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration) {
            // a valid token for testing purpose:
            //eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ3d3cuem9ycm90cmFja2VyLmNvbSIsImlhdCI6MTYwNjMzNjYwNywiZXhwIjoxNjM3ODcyNjA3LCJhdWQiOiJ3d3cuem9ycm90cmFja2VyLmNvbSIsInN1YiI6Im1vaGFtbWFkLnNha2hpZGVsIiwiR2l2ZW5OYW1lIjoiTW9oYW1tYWQiLCJTdXJuYW1lIjoiU2FraGlkZWwgSG92YXNpbiIsIkVtYWlsIjoianJvY2tldEBleGFtcGxlLmNvbSIsIlJvbGUiOlsiTWFuYWdlciIsIlByb2plY3QgQWRtaW5pc3RyYXRvciJdfQ.-bS2Y5jcp1fG9s5Ov-PkVHzgcRMHjPO5TUv7uIDthEk
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
        public static void AddAuthorizationPolicies(this IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy(Policies.CanCreateTracker, p => p.RequireClaim(Claims.IsAdmin));
            });
        }
        public static void AddAutoMapper(this IServiceCollection services) {
            IMapper mapper = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void AddTrackDbContext(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<DbContext, TrackDbContext>(options => {
                options.UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"));
            });
        }

    }
}
