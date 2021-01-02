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
using TrackLib.Commands;
using AutoMapper;
using TrackWorker.Services;

namespace TrackWorker.Extensions {
    public static class IServiceCollectionExtensions {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.SECTION_NAME));
        }
        public static void AddMessageListener(this IServiceCollection services) {
            services.AddSingleton<IMessageListener, MessageListener>();
            services.AddSingleton<ICommandListener, CommandListener>();
            services.AddSingleton<IUserListener, UserListener>();
        }
        public static void AddMiddlewares(this IServiceCollection services) {

            // Messages:
            services.AddSingleton<IGpsWatchLinkMiddleware, GpsWatchLinkMiddleware>();
            services.AddSingleton<IGpsWatchLocationMiddleware, GpsWatchLocationMiddleware>();
            services.AddSingleton<IGpsWatchAlarmMiddleware, GpsWatchAlarmMiddleware>();
            services.AddSingleton<IGpsWatchReplyMiddleware, GpsWatchReplyMiddleware>();

            // Commands:
            services.AddSingleton<IGpsWatchCommandMiddleware, SetValueCommandMiddleware>();
            services.AddSingleton<IGpsWatchQueryMiddleware, GpsWatchQueryMiddleware>();

        }
        public static void AddPipelines(this IServiceCollection services) {

            services.AddSingleton<IMessagePipeline>(sp => {
                var pipeline = new MessagePipeline();

                // Middlewares:
                pipeline.UseMiddleware<IGpsWatchLocationMiddleware>();
                pipeline.UseMiddleware<IGpsWatchAlarmMiddleware>();
                pipeline.UseMiddleware<IGpsWatchLinkMiddleware>();
                pipeline.UseMiddleware<IGpsWatchReplyMiddleware>();

                return pipeline;
            });

            services.AddSingleton<ICommandPipeline>(s => {
                var pipeline = new CommandPipeline();

                // Middlewares...
                pipeline.UseMiddleware<IGpsWatchCommandMiddleware>();
                pipeline.UseMiddleware<IGpsWatchQueryMiddleware>();

                return pipeline;
            });

        }
        public static void AddQueues(this IServiceCollection services) {
            services.AddSingleton<IMessageQueue, MessageQueue>();
            services.AddSingleton<ICommandQueue, CommandQueue>();
        }
        public static void AddRepositories(this IServiceCollection services) {
            services.AddScoped<ITrackerRepository, TrackerRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IAccessCodeRepository, AccessCodeRepository>();
        }
        public static void AddServices(this IServiceCollection services) {
            services.AddScoped<IAccessCodeService, AccessCodeService>();
            services.AddScoped<ITrackerService, TrackerService>();
            services.AddScoped<IReportService, ReportService>();
        }
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration) {
            // For dotnet ef tool:
            services.AddDbContext<TrackDbContext>(options => {
                options.UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"));
            });
            //^^^^^^^^^^^^^^^^^^^^

            //services.AddScoped(sp => {
            //    var options = new DbContextOptionsBuilder<TrackDbContext>()
            //        .UseMySQL(configuration.GetValue<string>("Database:ConnectionStrings:TrackDB"))
            //        .Options;
            //    return new TrackDbContext(options);
            //});
        }
        public static void AddHelpers(this IServiceCollection services) {
            services.AddTransient<GpsWatchCommandSet, GpsWatchCommandSet>();
        }
        public static void AddAutoMapper(this IServiceCollection services) {
            IMapper mapper = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
