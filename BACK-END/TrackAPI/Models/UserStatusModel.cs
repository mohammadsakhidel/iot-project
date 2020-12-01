using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class UserStatusModel {
        
        [Required]
        public string UserId { get; set; }

        [Required]
        public bool? IsActive { get; set; }

    }
}
