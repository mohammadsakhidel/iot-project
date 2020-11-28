using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackDataAccess.Database;
using TrackDataAccess.Repositories;
using TrackWorker.Listeners;
using TrackWorker.Helpers;
using TrackWorker.Processors.Queues;
using TrackWorker.Processors.Middlewares;
using TrackWorker.Processors.Pipelines;
using TrackWorker.Processors.Middlewares.Messages;
using TrackWorker.Processors.Middlewares.Commands;

namespace TrackWorker.Extensions {
    public static class IServiceCollectionExtensions {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.SECTION_NAME));
        }
        public static void AddMessageListener(this IServiceCollection services) {
            services.AddSingleton<IMessageListener, MessageListener>();
            services.AddSingleton<ICommandListener, CommandListener>();
        }
        public static void AddMiddlewares(this IServiceCollection services) {

            // Messages:
            services.AddTransient<ILinkMessageMiddleware, LinkMessageMiddleware>();
            services.AddTransient<ILocationMessageMiddleware, LocationMessageMiddleware>();
            services.AddTransient<IAlarmMessageMiddleware, AlarmMessageMiddleware>();
            services.AddTransient<IResponseMessageMiddleware, ResponseMessageMiddleware>();

            // Commands:
            services.AddTransient<ISendValueCommandMiddleware, SetValueCommandMiddleware>();
            services.AddTransient<IQueryCommandMiddleware, QueryCommandMiddleware>();

        }
        public static void AddPipelines(this IServiceCollection services) {

            services.AddSingleton<IMessagePipeline>(sp => {
                var pipeline = new MessagePipeline();

                // Middlewares:
                pipeline.UseMiddleware<ILocationMessageMiddleware>();
                pipeline.UseMiddleware<IAlarmMessageMiddleware>();
                pipeline.UseMiddleware<ILinkMessageMiddleware>();
                pipeline.UseMiddleware<IResponseMessageMiddleware>();

                return pipeline;
            });

            services.AddSingleton<ICommandPipeline>(s => {
                var pipeline = new CommandPipeline();

                // Middlewares...
                pipeline.UseMiddleware<ISendValueCommandMiddleware>();
                pipeline.UseMiddleware<IQueryCommandMiddleware>();

                return pipeline;
            });

        }
        public static void AddQueues(this IServiceCollection services) {
            services.AddSingleton<IMessageQueue, MessageQueue>();
            services.AddSingleton<ICommandQueue, CommandQueue>();
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

            services.AddTransient<TrackDbContext>(sp => {
                var options = new DbContextOptionsBuilder<TrackDbContext>()
                    .UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"))
                    .Options;
                return new TrackDbContext(options);
            });
        }

    }
}
