using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class ExecuteCommandDto {
        public string TrackerId { get; set; }
        public string CommandType { get; set; }
        public string Payload { get; set; }
    }
}
