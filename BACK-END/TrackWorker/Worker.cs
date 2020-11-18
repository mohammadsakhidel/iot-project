using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TrackWorker {
    public class Worker : BackgroundService {

        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

            var portNumber = _configuration.GetValue<int>("Socket:PortNumber");
            var backlogSize = _configuration.GetValue<int>("Socket:ConnectionBacklogSize");
            var ipAddress = IPAddress.Parse("127.0.0.1");
            
            var ipe = new IPEndPoint(ipAddress, portNumber);

            // Start Listening the Port:
            using var listener = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipe);
            listener.Listen(backlogSize);

            while (!stoppingToken.IsCancellationRequested) {
                try {

                    var socket = listener.Accept();
                    _logger.LogInformation("Connection accepted...");

                    await Task.Delay(10000);
                } catch (Exception ex) {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
