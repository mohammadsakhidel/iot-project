﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrackDataAccess.Database;
using TrackDataAccess.Repositories;
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
            services.AddTransient<ILocationMessageMiddleware, LocationMessageMiddleware>();
            services.AddTransient<IAlarmMessageMiddleware, AlarmMessageMiddleware>();
        }
        public static void AddPipelines(this IServiceCollection services) {
            services.AddSingleton<IInPipeline>(sp => {
                var pipeline = new InPipeline();

                // Middlewares:
                pipeline.UseMiddleware<ILocationMessageMiddleware>();
                pipeline.UseMiddleware<ILinkMessageMiddleware>();
                pipeline.UseMiddleware<IAlarmMessageMiddleware>();

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
        public static void AddRepositories(this IServiceCollection services) {
            services.AddTransient<ITrackerRepository, TrackerRepository>();
            services.AddTransient<ILocationReportRepository, LocationReportRepository>();
            services.AddTransient<IAlarmReportRepository, AlarmReportRepository>();
        }
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration) {
            // For dotnet ef tool:
            services.AddDbContext<TrackDbContext>(options => {
                options.UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"));
            });
            //^^^^^^^^^^^^^^^^^^^^

            services.AddTransient<DbContext, TrackDbContext>(sp => {
                var options = new DbContextOptionsBuilder<TrackDbContext>()
                    .UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"))
                    .Options;
                return new TrackDbContext(options);
            });
        }

    }
}
