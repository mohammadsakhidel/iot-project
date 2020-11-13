using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects.Commands
{
    public class TerminalBlindSpotCommand : Command
    {
        #region Ctors:
        public TerminalBlindSpotCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData)
            : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
            if (!string.IsNullOrEmpty(commandData))
            {
                LocationData = new LocationData(commandData);
            }
        }
        #endregion

        #region Props:
        public LocationData LocationData { get; private set; }
        #endregion
    }
}
