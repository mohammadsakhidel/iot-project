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

        [Required]
        [MaxLength(32)]
        [Column("product_type")]
        public string ProductType { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("product_model")]
        public string ProductModel { get; set; }

        /// <summary>
        /// Owner user's ID.
        /// </summary>
        [MaxLength(64)]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(16)]
        [Column("command_set")]
        public string CommandSet { get; set; }

        [MaxLength(16)]
        [Column("status")]
        public string Status { get; set; }

        [Column("last_connection")]
        public DateTime? LastConnection { get; set; }

        [MaxLength(32)]
        [Column("last_connected_server")]
        public string LastConnectedServer { get; set; }

        [MaxLength(512)]
        [Column("explanation")]
        public string Explanation { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("display_name")]
        public string DisplayName { get; set; }

        [MaxLength(64)]
        [Column("icon_image_id")]
        public string IconImageId { get; set; }

        [MaxLength(64)]
        [Column("default_icon")]
        public string DefaultIcon { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("serial_number")]
        public string SerialNumber { get; set; }

        #region Navigation Props:
        public List<TrackerUser> Users { get; set; }
        public List<TrackerPermittedUser> PermittedUsers { get; set; }
        #endregion

    }
}