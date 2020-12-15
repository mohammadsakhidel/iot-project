using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class TrackerSearchDto {
        public string Manufacturer { get; set; }
        public string RawID { get; set; }
        public string UserId { get; set; }
        public string SerialNumber { get; set; }
    }
}
