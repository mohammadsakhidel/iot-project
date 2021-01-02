using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using TrackWorker.Extensions;

namespace TrackWorker {
    public class Program {

        public static IServiceProvider Services { get; private set; }

        public static void Main(string[] args) {
            IHost host = CreateHostBuilder(args).Build();

            // Main scope:
            var scope = host.Services.CreateScope();
            Services = scope.ServiceProvider;

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddEventLog(new EventLogSettings {
                        SourceName = "TrackWorker"
                    });
                })
                .UseWindowsService()
                .ConfigureServices((hostContext, services) => {

                    services.AddHostedService<Worker>();

                    services.ConfigureSettings(hostContext.Configuration);

                    services.AddMessageListener();
                    services.AddMiddlewares();
                    services.AddPipelines();
                    services.AddQueues();

                    services.AddDbContext(hostContext.Configuration);
                    services.AddRepositories();
                    services.AddServices();

                    services.AddHelpers();
                    services.AddAutoMapper();

                });
    }
}
