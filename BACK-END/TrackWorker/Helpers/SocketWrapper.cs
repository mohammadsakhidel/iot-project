using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Helpers {
    public class SocketWrapper : ISocketWrapper {
        private readonly Socket _socket;
        public SocketWrapper(Socket socket) {
            _socket = socket;
        }

        public int Send(byte[] bytes) {
            return _socket.Send(bytes);
        }

        public Socket GetRealSocket() {
            return _socket;
        }
    }
}
