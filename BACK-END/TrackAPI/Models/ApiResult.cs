using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class ApiResult {
        public bool Done { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }
    }
}
