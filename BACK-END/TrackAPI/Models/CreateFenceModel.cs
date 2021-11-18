using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class CreateFenceModel {
        public string TrackerId { get; set; }
        public string FenceData { get; set; } // Format: lat1,lng1;lat2,lng2;...
    }
}
