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
    }
}
