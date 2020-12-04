using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class CommandLogDto {
        public int Id { get; set; }
        public string Type { get; set; }
        public string TrackerId { get; set; }
        public string Payload { get; set; }
        public string UserId { get; set; }
        public string Response { get; set; }
        public string CreationTime { get; set; }
    }
}
