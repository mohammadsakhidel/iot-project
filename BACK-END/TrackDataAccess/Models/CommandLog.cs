using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {

    [Table("command_logs")]
    public class CommandLog : Entity {

        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        [Column("type")]
        public string Type { get; set; }

        [Required]
        [MaxLength(32)]
        [Column("tracker_id")]
        public string TrackerId { get; set; }

        [MaxLength(256)]
        [Column("payload")]
        public string Payload { get; set; }

        [Required]
        [MaxLength(64)]
        [Column("user_id")]
        public string UserId { get; set; }

        [MaxLength(512)]
        [Column("response")]
        public string Response { get; set; }

    }
}
