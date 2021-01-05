using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class MessageModel {
        public int Id { get; set; }
        public string TrackerId { get; set; }
        public string CreationTime { get; set; }
    }
}
