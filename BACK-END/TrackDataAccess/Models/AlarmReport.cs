using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models.Base;

namespace TrackDataAccess.Models {
    [Table("alarm_reports")]
    public class AlarmReport : TrackerReportData {

        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        [ForeignKey("Tracker")]
        [Column("tracker_id")]
        public string TrackerId { get; set; }

        #region Navigation Properties:
        public Tracker Tracker { get; set; }
        #endregion

    }
}
