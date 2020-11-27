using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
using TrackAPI.Middlewares;
using TrackAPI.Services;
using TrackDataAccess.Database;
using TrackDataAccess.Models.Identity;

namespace TrackAPI.Extensions {
    public static class IServiceCollectionExtensions {

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration) {
            services.AddSingleton<IJwtAuthenticationMiddleware, JwtAuthenticationMiddleware>();
            // sample admin token for testing porpuse:
            //eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ3d3cuem9ycm90cmFja2VyLmNvbSIsImlhdCI6MTYwNjQ2MzI0MCwiZXhwIjoxNjM3OTk5MjQwLCJhdWQiOiJ3d3cuem9ycm90cmFja2VyLmNvbSIsInN1YiI6Im1vaGFtbWFkLnNha2hpZGVsIiwiR2l2ZW5OYW1lIjoiTW9oYW1tYWQiLCJTdXJuYW1lIjoiU2FraGlkZWwiLCJFbWFpbCI6Im1vaGFtbWFkLnNha2hpZGVsQGdtYWlsLmNvbSIsImlzYWRtIjoidHJ1ZSJ9.SdJnT3SSA0YS8PssoqcQB7Ia_yDHNfly6Tm4gt9K1_w
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy(Policies.CanCreateUser, p => p.RequireClaim(ClaimNames.ISADMIN));
                options.AddPolicy(Policies.CanReadUser, p => p.RequireClaim(ClaimNames.ISADMIN));
                options.AddPolicy(Policies.CanDeleteUser, p => p.RequireClaim(ClaimNames.ISADMIN));
                options.AddPolicy(Policies.CanUpdateUser, p => p.RequireClaim(ClaimNames.ISADMIN));
            });
        }

        public static void AddAutoMapper(this IServiceCollection services) {
            IMapper mapper = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddTrackDbContext(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<TrackDbContext>(options => {
                options.UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"));
            });
        }

        public static void AddAspNetIdentity(this IServiceCollection services) {

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<TrackDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
            });

        }

        public static void AddAPIServices(this IServiceCollection services) {
            services.AddScoped<IUserService, UserService>();
        }

    }
}
