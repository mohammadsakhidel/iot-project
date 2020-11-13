using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformRemoveAlarmSetCommand : Command
    {
        public PlatformRemoveAlarmSetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformRemoveAlarmSetCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.REMOVE, commandData)
        {

        }
    }
}
