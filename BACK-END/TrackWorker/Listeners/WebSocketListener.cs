using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Events;
using Fleck;
using TrackWorker.Helpers;

namespace TrackWorker.Listeners {
    public class WebSocketListener : IWebSocketListener {

        public event EventHandler<WebSocketClientConnectedEventArgs> OnClientConnected;
        public event EventHandler<WebSocketClientDisconnectedEventArgs> OnClientDisconnected;
        public event EventHandler<WebSocketDataReceivedEventArgs> OnDataReceived;

        public void StartListening(int portNumber) {
            var server = new WebSocketServer($"ws://0.0.0.0:{portNumber}");
            server.Start(socket => {

                socket.OnOpen = () => {
                    OnClientConnected?.Invoke(this, new WebSocketClientConnectedEventArgs {
                        Client = new WebSocketClient { Socket = socket }
                    });
                };

                socket.OnClose = () => {
                    OnClientDisconnected?.Invoke(this, new WebSocketClientDisconnectedEventArgs {
                        Client = new WebSocketClient { Socket = socket }
                    });
                };

                socket.OnMessage = message => {
                    OnDataReceived?.Invoke(this, new WebSocketDataReceivedEventArgs {
                        Client = new WebSocketClient { Socket = socket },
                        Message = message
                    });
                };

            });
        }
    }
}
