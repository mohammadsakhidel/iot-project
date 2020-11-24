using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TrackWorker.Models {
    public class TrackerConnection {
        public Socket Socket { get; set; }
        /// <summary>
        /// Stores trackers replies to sent commands in base64 encoded format
        /// </summary>
        public BlockingCollection<string> ResponseQueue { get; set; } = new BlockingCollection<string>();
    }
}
