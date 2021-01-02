using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class AccessCodeModel {

        public string Id { get; set; }

        public string UserId { get; set; }

        public string TrackerId { get; set; }

        public DateTime CreationTime { get; set; }

    }
}
