using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackWorker.Models {
    public class TrackerModel {
        public string Id { get; set; }

        [Required]
        public string RawID { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string CommandSet { get; set; }

        public string Explanation { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string IconImageId { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public string UserId { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public string ProductModel { get; set; }
        public DateTime CreationTime { get; set; }
        public string Status { get; set; }
        public DateTime? LastConnection { get; set; }
        public string LastConnectedServer { get; set; }

        // Navigatio Props:
        public List<TrackerUserModel> Users { get; set; }
    }
}
