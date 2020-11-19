using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;

namespace TrackWorker {
    public static class Globals {
        public static BlockingCollection<Message> IncomingMessages { get; set; }
        public static BlockingCollection<Message> OutgoingMessages { get; set; }
    }
}
