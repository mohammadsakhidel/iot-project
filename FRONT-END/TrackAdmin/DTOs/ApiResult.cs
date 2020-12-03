using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class ApiResult {
        public bool Done { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }
    }
}
