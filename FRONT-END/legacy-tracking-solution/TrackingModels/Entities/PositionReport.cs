using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Entities
{
    /// <summary>
    /// Command #2 in the document.
    /// </summary>
    public class PositionReport
    {
        public long ID { get; set; }

        [Required]
        [MaxLength(16)]
        public string TerminalID { get; set; }

        [Required]
        public long LocationDataID { get; set; }

        [Required]
        public DateTime SaveTimeUtc { get; set; }

        #region Navigations:
        public virtual Terminal Terminal { get; set; }
        public virtual LocationData LocationData { get; set; }
        #endregion

    }
}
