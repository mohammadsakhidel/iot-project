using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {
    [Table("trackers")]
    public class Tracker : SoftEntity {

        [Key]
        [Required]
        [MaxLength(16)]
        [Column("id")]
        public string Id { get; set; }

        [Required]
        [MaxLength(32)]
        [Column("device_type")]
        public string DeviceType { get; set; }

        [Column("associated_product_id")]
        public int? AssociatedProductId { get; set; }

        [Column("last_connection")]
        public DateTime? LastConnection { get; set; }

        [MaxLength(32)]
        [Column("last_connected_server")]
        public string LastConnectedServer { get; set; }

    }
}
