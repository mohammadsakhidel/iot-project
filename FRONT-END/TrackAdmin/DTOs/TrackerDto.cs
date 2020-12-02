using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class TrackerDto {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Manufacturer { get; set; }
        public string RawID { get; set; }
        public string CommandSet { get; set; }
        public string CreationTime { get; set; }

    }
}
