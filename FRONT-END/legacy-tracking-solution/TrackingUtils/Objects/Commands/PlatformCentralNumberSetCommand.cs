using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformCentralNumberSetCommand : Command
    {
        public PlatformCentralNumberSetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformCentralNumberSetCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.CENTER, commandData)
        {

        }
    }
}
