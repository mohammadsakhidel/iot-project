using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Listeners;
using TrackWorker.Models;

namespace TrackWorker.Extensions {
    public static class IServiceCollectionExtensions {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.SECTION_NAME));
        }
        public static void AddMessageListener(this IServiceCollection services) {
            services.AddSingleton<IMessageListener, MessageListener>();
        }
    }
}
