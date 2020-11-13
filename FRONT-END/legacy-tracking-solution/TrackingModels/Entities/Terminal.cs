using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Entities
{
    public class Terminal
    {
        [Required]
        [MaxLength(16)]
        public string ID { get; set; }

        [Required]
        [MaxLength(32)]
        public string DisplayName { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(16)]
        public string Product { get; set; }

        [Required]
        public DateTime LastActivityTimeUtc { get; set; }

        [Required]
        [MaxLength(32)]
        public string Model { get; set; }

        [Required]
        [MaxLength(16)]
        public string Variant { get; set; }

        [MaxLength(1024)]
        public string Statements { get; set; }

        [Required]
        public DateTime SaveTimeUtc { get; set; }

        #region Terminal Configs:
        [Required]
        [MaxLength(4)]
        public string ManufacturerID { get; set; }

        [Required]
        [MaxLength(16)]
        public string DeviceID { get; set; }

        [MaxLength(32)]
        [Required]
        public string IMEI { get; set; }

        [Required]
        [MaxLength(32)]
        public string ServerAddress { get; set; }

        [Required]
        public int ServerPort { get; set; }

        [MaxLength(64)]
        public string Version { get; set; }

        [MaxLength(16)]
        public string CenterNumber { get; set; }

        [MaxLength(16)]
        public string SOS1 { get; set; }

        [MaxLength(16)]
        public string SOS2 { get; set; }

        [MaxLength(16)]
        public string SOS3 { get; set; }

        public int? UploadIntervalSeconds { get; set; }

        public double? LastStatedBatteryLevel { get; set; }

        [MaxLength(16)]
        public string Language { get; set; }

        [MaxLength(8)]
        public string TimeZone { get; set; }
        #endregion

        #region Navigation Props:
        public virtual Customer Customer { get; set; }
        #endregion
    }
}
