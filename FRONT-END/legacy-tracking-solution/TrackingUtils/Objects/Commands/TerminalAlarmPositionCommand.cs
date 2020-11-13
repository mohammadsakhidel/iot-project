using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class TerminalAlarmPositionCommand : Command
    {
        #region Ctors:
        public TerminalAlarmPositionCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData)
            : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
            if (!string.IsNullOrEmpty(commandData))
            {
                LocationData = new LocationData(commandData);
            }
        }
        #endregion

        #region Overrides:
        public override Command GenerateReply()
        {
            var reply = new TerminalAlarmPositionCommand(Manufacturer, DeviceID, 2, CommandTypes.AL, "");
            return reply;
        }
        #endregion

        #region Props:
        public LocationData LocationData { get; private set; }
        #endregion
    }
}
