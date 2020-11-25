using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TrackWorker.Helpers {
    public interface ISocketWrapper {
        int Send(byte[] bytes);
        Socket GetRealSocket();
    }
}
