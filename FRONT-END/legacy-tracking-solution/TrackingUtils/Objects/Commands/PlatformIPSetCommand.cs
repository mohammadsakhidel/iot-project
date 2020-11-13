using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformIPSetCommand : Command
    {
        public PlatformIPSetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformIPSetCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.IP, commandData)
        {

        }
    }
}
