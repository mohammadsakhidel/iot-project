using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataAccess.Models.Identity {
    public class AppUser : IdentityUser {
        [Required]
        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("delete_time")]
        public DateTime? DeleteTime { get; set; }

        public List<IdentityUserClaim<string>> Claims { get; set; }
    }
}
