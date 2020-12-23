﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Identity;

namespace TrackDataAccess.Models {

    [Table("tracker_allowed_users")]
    public class TrackerAllowedUser {

        [Required]
        [MaxLength(32)]
        [Column("tracker_id")]
        public string TrackerId { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("user_id")]
        public string UserId { get; set; }

        public AppUser User { get; set; }
        public Tracker Tracker { get; set; }

    }
}