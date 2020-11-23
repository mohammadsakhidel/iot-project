using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackDataAccess.Models.Base {
    public abstract class TrackerReportData : Entity {
        [Required]
        [Column("report_time")]
        public DateTime ReportTime { get; set; }

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
    }
}
