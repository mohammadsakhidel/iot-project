using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Listeners;
using TrackWorker.Models;
using TrackWorker.Processors;
using TrackWorker.Processors.LineManagers;
using TrackWorker.Processors.Middlewares;
using TrackWorker.Processors.Pipelines;

namespace TrackWorker.Extensions {
    public static class IServiceCollectionExtensions {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.SECTION_NAME));
        }
        public static void AddMessageListener(this IServiceCollection services) {
            services.AddSingleton<IIncomingMessageListener, IncomingMessageListener>();
            services.AddSingleton<IOutgoingMessageListener, OutgoingMessageListener>();
        }
        public static void AddMiddlewares(this IServiceCollection services) {
            services.AddTransient<ILinkMessageMiddleware, LinkMessageMiddleware>();
        }
        public static void AddPipelines(this IServiceCollection services) {
            services.AddSingleton<IInPipeline>(s => {
                var pipeline = new InPipeline();
                
                // Middlewares:
                pipeline.UseMiddleware<ILinkMessageMiddleware>();

                return pipeline;
            });

            services.AddSingleton<IOutPipeline>(s => {
                var pipeline = new OutPipeline();
                // Middlewares...
                return pipeline;
            });
        }
        public static void AddLineManagers(this IServiceCollection services) {
            services.AddSingleton<IInLineManager, InLineManager>();
            services.AddSingleton<IOutLineManager, OutLineManager>();
        }
    }
}
