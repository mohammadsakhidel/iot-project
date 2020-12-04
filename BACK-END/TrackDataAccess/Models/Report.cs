using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {
    [Table("reports")]
    public class Report : Entity {

        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        [Column("report_type")]
        public string ReportType { get; set; }

        [Required]
        [Column("report_time")]
        public DateTime ReportTime { get; set; }

        [Required]
        [MaxLength(16)]
        [ForeignKey("Tracker")]
        [Column("tracker_id")]
        public string TrackerId { get; set; }

        [Required]
        [Column("latitude")]
        public double Latitude { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("latitude_mark")]
        public string LatitudeMark { get; set; }

        [Required]
        [Column("longitude")]
        public double Longitude { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("longitude_mark")]
        public string LongitudeMark { get; set; }

        [Column("is_valid")]
        public bool IsValid { get; set; }

        [Required]
        [Column("speed")]
        public double Speed { get; set; }

        [Required]
        [Column("direction")]
        public double Direction { get; set; }

        [Column("altitude")]
        public double? Altitude { get; set; }

        [Column("signal_strength")]
        public double? SignalStrength { get; set; }

        [Column("battery")]
        public double? Battery { get; set; }

        [MaxLength(64)]
        [Column("tracker_state")]
        public string TrackerState { get; set; }

        #region Navigation Properties:
        public Tracker Tracker { get; set; }
        #endregion

    }
}
