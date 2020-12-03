using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class TrackerSearchModel {
        public string UserId { get; set; }
        public string Manufacturer { get; set; }
        public string RawID { get; set; }
    }
}
