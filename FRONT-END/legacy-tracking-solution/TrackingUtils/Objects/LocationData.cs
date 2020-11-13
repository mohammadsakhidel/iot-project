using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects
{
    public class LocationData
    {
        #region Ctors:
        public LocationData(string commandData)
        {
            var data = commandData.Split(',');
            DateTime = DateTime.ParseExact($"{data[0]}{data[1]}", "ddMMyyHHmmss", CultureInfo.InvariantCulture);
            Latitude = Convert.ToDouble(data[3]);
            LatitudeMark = data[4];
            Longitude = Convert.ToDouble(data[5]);
            LongitudeMark = data[6];
            Speed = Convert.ToDouble(data[7]);
            Direction = Convert.ToDouble(data[8]);
            Altitude = Convert.ToDouble(data[9]);
            GsmSignalStrengh = Convert.ToDouble(data[11]);
            Power = Convert.ToDouble(data[12]);
            TerminalState = new TerminalState(data[15]);
            Accuracy = Convert.ToDouble(data.Last());
        }
        #endregion

        #region Props:
        public DateTime DateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LatitudeMark { get; set; }
        public string LongitudeMark { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public double Altitude { get; set; }
        public double GsmSignalStrengh { get; set; }
        public double Power { get; set; }
        public TerminalState TerminalState { get; set; }
        public double Accuracy { get; set; }
        #endregion
    }
}
