using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class TrackerPermittedUserModel {
        
        [Required]
        public string TrackerId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Permissions { get; set; }

        public UserModel User { get; set; }

    }
}
