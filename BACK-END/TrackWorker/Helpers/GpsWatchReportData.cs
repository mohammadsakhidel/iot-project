using System;
using System.Collections.Generic;
using System.Text;
using TrackLib.Utils;

namespace TrackWorker.Helpers {
    public class GpsWatchReportData {
        private GpsWatchReportData() {

        }

        public string ReportType { get; set; }
        public DateTime ReportTime { get; set; }
        public bool IsValid { get; set; }
        public double Latitude { get; set; }
        /// <summary>
        /// N for North, S for South
        /// </summary>
        public char LatitudeMark { get; set; }
        public double Longitude { get; set; }
        /// <summary>
        /// E for East, W for West
        /// </summary>
        public char LongitudeMark { get; set; }
        /// <summary>
        /// Moving speed in Kilometers pir Hout unit.
        /// </summary>
        public double Speed { get; set; }
        /// <summary>
        /// Measured in Degrees, e.g. 152
        /// </summary>
        public double Direction { get; set; }
        /// <summary>
        /// Measured in Meters
        /// </summary>
        public double Altitude { get; set; }
        /// <summary>
        /// Shows the battery charge percentage of the device.
        /// </summary>
        public double Power { get; set; }
        /// <summary>
        /// Network signal in percent.
        /// </summary>
        public double SignalStrength { get; set; }
        public string TrackerStateHex { get; set; }
        /// <summary>
        /// 8 Hex characters representing 32 bits of data.
        /// The high 16bit expression alarming, low 16bit expression condition.
        /// Bit[0] -> Low battery state
        /// Bit[1] -> Out of fence state
        /// Bit[2] -> Into the fence state
        /// Bit[3] -> Watch state
        /// Bit[16] -> SOS alarm
        /// Bit[17] -> Low battery alarm
        /// Bit[18] -> Out fence alarm
        /// Bit[19] -> Into the fence alarm
        /// Bit[20] -> Remove the watch alarm
        /// </summary>
        public string TrackerStateBinary {
            get {
                int bits = 32;
                return TextUtil.HexToBinaryString(TrackerStateHex).PadLeft(bits, '0');
            }
        }

        public static GpsWatchReportData FromArray(string[] array) {
            var message = new GpsWatchReportData();

            message.ReportType = array[0];

            #region TimeOfCapture:
            var baseYear = 2000;
            var dateString = array[1];
            var timeString = array[2];
            (int day, int month, int year) = (
                int.Parse(dateString[0..2]),
                int.Parse(dateString[2..4]),
                int.Parse(dateString[4..6])
            );
            (int hour, int minute, int second) = (
                int.Parse(timeString[0..2]),
                int.Parse(timeString[2..4]),
                int.Parse(timeString[4..6])
            );
            message.ReportTime = new DateTime(year + baseYear, month, day, hour, minute, second);
            #endregion

            message.IsValid = array[3] == "A";

            #region Latitude & Longitude:
            message.Latitude = Convert.ToDouble(array[4]);
            message.LatitudeMark = !string.IsNullOrEmpty(array[5]) ? array[5][0] : '0';
            message.Longitude = Convert.ToDouble(array[6]);
            message.LongitudeMark = !string.IsNullOrEmpty(array[7]) ? array[7][0] : '0';
            #endregion

            message.Speed = Convert.ToDouble(array[8]);
            message.Direction = Convert.ToDouble(array[9]);
            message.Altitude = Convert.ToDouble(array[10]);
            message.SignalStrength = Convert.ToDouble(array[12]);
            message.Power = Convert.ToDouble(array[13]);
            message.TrackerStateHex = array[16];

            return message;
        }
    }
}
