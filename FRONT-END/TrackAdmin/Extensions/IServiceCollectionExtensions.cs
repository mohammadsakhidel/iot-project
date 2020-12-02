using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackAdmin.Services;
using TrackAdmin.ViewModels;
using TrackAdmin.Views;

namespace TrackAdmin.Extensions {
    public static class IServiceCollectionExtensions {

        public static void AddConfiguration(this IServiceCollection services) {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            var configuration = configBuilder.Build();

            services.AddSingleton<IConfiguration>(configuration);
        }

        public static void AddWindows(this IServiceCollection services) {
            services.AddSingleton<MainWindow>(sp => {
                return new MainWindow {
                    DataContext = new MainWindowViewModel()
                };
            });
        }

        public static void AddServices(this IServiceCollection services) {
            services.AddSingleton<IUserService, UserService>();
        }

        public static void AddViewModels(this IServiceCollection services) {
            services.AddSingleton<IUsersViewModel, UsersViewModel>();
            services.AddSingleton<IUserEditorViewModel, UserEditorViewModel>();
            services.AddSingleton<ISearchUsersViewModel, SearchUsersViewModel>();

            services.AddSingleton<ITrackersViewModel, TrackersViewModel>();
            services.AddSingleton<ITrackerLogsViewModel, TrackerLogsViewModel>();
            services.AddSingleton<ITrackerTestingViewModel, TrackerTestingViewModel>();
        }

        public static void AddHelpers(this IServiceCollection services) {
            
            // HttpClient:
            services.AddTransient<HttpClient>(sp => {
                var config = (IConfiguration)sp.GetService(typeof(IConfiguration));
                HttpClientHandler handler = new HttpClientHandler {
                    ServerCertificateCustomValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => true
                };
                var http = new HttpClient(handler);
                http.AddAuthHeader(config["API:Token"]);
                return http;
            });

        }
        
    }
}