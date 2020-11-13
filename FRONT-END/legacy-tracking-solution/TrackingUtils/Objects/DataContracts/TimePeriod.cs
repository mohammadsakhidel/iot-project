using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects.DataContracts
{
    [DataContract]
    public class TimePeriod
    {
        #region Ctors:
        public TimePeriod(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public TimePeriod(string startTime, string endTime)
        {
            var s = startTime.Split(':');
            var e = endTime.Split(':');
            var now = DateTime.Now;

            StartTime = new DateTime(now.Year, now.Month, now.Day, Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), 0);
            EndTime = new DateTime(now.Year, now.Month, now.Day, Convert.ToInt32(e[0]), Convert.ToInt32(e[1]), 0);
        }
        #endregion

        #region Props:
        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }
        #endregion

        #region Overrides:
        public override string ToString()
        {
            return $"{StartTime.ToString("HH:mm")}-{EndTime.ToString("HH:mm")}";
        }
        #endregion
    }
}
