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
        [MaxLength(16)]
        [Column("raw_id")]
        public string RawID { get; set; }

        [Required]
        [MaxLength(8)]
        [Column("manufacturer")]
        public string Manufacturer { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; }

        [MaxLength(64)]
        [Column("product_model")]
        public string ProductModel { get; set; }

        [MaxLength(64)]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(16)]
        [Column("command_set")]
        public string CommandSet { get; set; }

        [Column("last_connection")]
        public DateTime? LastConnection { get; set; }

        [MaxLength(32)]
        [Column("last_connected_server")]
        public string LastConnectedServer { get; set; }

        [MaxLength(512)]
        [Column("explanation")]
        public string Explanation { get; set; }

    }
}