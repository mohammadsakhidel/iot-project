using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class TrackerModel {
        public string Id { get; set; }

        [Required]
        public string RawID { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string CommandSet { get; set; }

        public string Explanation { get; set; }

        public string UserId { get; set; }
        public int? ProductId { get; set; }
        public string ProductModel { get; set; }
        public string CreationTime { get; set; }
        public DateTime? LastConnection { get; set; }
        public string LastConnectedServer { get; set; }
    }
}
