using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects
{
    public class TerminalState
    {
        #region Ctors:
        public TerminalState(string hexState)
        {
            _hexState = hexState;
            _binaryState = Convert.ToString(Convert.ToInt32(hexState, 16), 2).PadLeft(32, '0');

            LowBatteryState = _binaryState[0] == 1;
            OutOfFenceState = _binaryState[1] == 1;
            IntoTheFenceState = _binaryState[2] == 1;
            WatchState = _binaryState[3] == 1;

            SosAlarm = _binaryState[16] == 1;
            LowBatteryAlarm = _binaryState[17] == 1;
            OutOfFenceAlarm = _binaryState[18] == 1;
            IntoTheFenceAlarm = _binaryState[19] == 1;
            RemoveWatchAlarm = _binaryState[20] == 1;
        }
        #endregion

        #region Fields:
        string _binaryState = "00000000";
        string _hexState;
        #endregion

        #region Props:
        public bool LowBatteryState { get; private set; }
        public bool OutOfFenceState { get; private set; }
        public bool IntoTheFenceState { get; private set; }
        public bool WatchState { get; private set; }

        public bool SosAlarm { get; private set; }
        public bool LowBatteryAlarm { get; private set; }
        public bool OutOfFenceAlarm { get; private set; }
        public bool IntoTheFenceAlarm { get; private set; }
        public bool RemoveWatchAlarm { get; private set; }
        #endregion

        #region Overrides:
        public override string ToString()
        {
            return _hexState;
        }
        #endregion
    }
}
