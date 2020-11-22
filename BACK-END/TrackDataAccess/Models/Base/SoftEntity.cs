using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TrackDataAccess.Models.Base {
    public class SoftEntity : Entity {

        [Required]
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("delete_time")]
        public DateTime DeleteTime { get; set; }

    }
}
