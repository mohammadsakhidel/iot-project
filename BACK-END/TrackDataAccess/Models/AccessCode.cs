using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {

    [Table("access_codes")]
    public class AccessCode : Entity {

        [Key]
        [Required]
        [MaxLength(64)]
        [Column("id")]
        public string Id { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(16)]
        [Column("tracker_id")]
        public string TrackerId { get; set; }

    }
}
