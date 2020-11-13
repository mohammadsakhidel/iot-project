using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class TerminalDto
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public int CustomerID { get; set; }
        public string Product { get; set; }
        public DateTime LastActivityTimeUtc { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public string Statements { get; set; }
        public DateTime SaveTimeUtc { get; set; }

        #region Terminal Configs:
        public string ManufacturerID { get; set; }
        public string DeviceID { get; set; }
        public string IMEI { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public string Version { get; set; }
        public string CenterNumber { get; set; }
        public string SOS1 { get; set; }
        public string SOS2 { get; set; }
        public string SOS3 { get; set; }
        public int? UploadIntervalSeconds { get; set; }
        public double? LastStatedBatteryLevel { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        #endregion

        #region Navigation Props:
        public CustomerDto Customer { get; set; }
        #endregion
    }
}
