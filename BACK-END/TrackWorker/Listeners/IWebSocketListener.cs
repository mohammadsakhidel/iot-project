using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Events;

namespace TrackWorker.Listeners {
    public interface IWebSocketListener {
        // Events:
        public event EventHandler<WebSocketClientConnectedEventArgs> OnClientConnected;
        public event EventHandler<WebSocketClientDisconnectedEventArgs> OnClientDisconnected;
        public event EventHandler<WebSocketDataReceivedEventArgs> OnDataReceived;

        // Methods:
        void StartListening(int portNumber);
    }
}
