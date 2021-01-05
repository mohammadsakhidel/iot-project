using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackWorker.Models {
    public class MessageModel {
        public int Id { get; set; }
        public string TrackerId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
