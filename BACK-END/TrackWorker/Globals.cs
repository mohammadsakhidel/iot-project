using System;
using System.Collections.Generic;
using System.Text;
using TrackWorker.Models;

namespace TrackWorker {
    public static class Globals {
        public static Queue<TerminalMessage> MessageQueue { get; set; }
        public static Queue<TerminalCommand> CommandQueue { get; set; }
    }
}
