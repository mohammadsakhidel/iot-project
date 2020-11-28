using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using TrackAPI.Helpers;
using TrackLib.DataContracts;

namespace TrackAPI.Sockets {
    public class CommandExecutor : ICommandExecutor {

        private readonly AppSettings _appSettings;
        public CommandExecutor(IOptions<AppSettings> options) {
            _appSettings = options.Value;
        }

        public Task<CommandResponse> SendAsync(CommandRequest request, string host = "") {
            return Task.Run(() => {
                try {
                    // Send Command:
                    using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect((!string.IsNullOrEmpty(host) ? host : _appSettings.Worker.DefaultServer), _appSettings.Worker.PortNumber);
                    int bytesSent = socket.Send(request.Serialize());
                    if (bytesSent <= 0)
                        return null;

                    // Wait for the response:
                    var buffer = new byte[_appSettings.Worker.BufferSize];
                    socket.ReceiveTimeout = _appSettings.Worker.ResponseTimeoutMillis;
                    var bytesReceived = socket.Receive(buffer);
                    if (bytesReceived <= 0)
                        return null;

                    // Deserialize received bytes:
                    var responseBytes = new byte[bytesReceived];
                    Array.Copy(buffer, 0, responseBytes, 0, bytesReceived);
                    var response = CommandResponse.Deserialize(responseBytes);
                    return response;
                } catch {
                    return null;
                }
            });
        }
    }
}
