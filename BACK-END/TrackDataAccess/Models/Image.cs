using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {

    [Table("images")]
    public class Image : Entity {

        [Key]
        [Required]
        [MaxLength(64)]
        [Column("id")]
        public string Id { get; set; }

        [Required]
        [MaxLength(32)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("bytes", TypeName = "MEDIUMBLOB")]
        public byte[] Bytes { get; set; }

        [Required]
        [Column("width")]
        public int Width { get; set; }

        [Required]
        [Column("height")]
        public int Height { get; set; }

    }
}
