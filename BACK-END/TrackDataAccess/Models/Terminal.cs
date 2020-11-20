using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {
    [Table("terminals")]
    public class Terminal : IDEntity {
        
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
