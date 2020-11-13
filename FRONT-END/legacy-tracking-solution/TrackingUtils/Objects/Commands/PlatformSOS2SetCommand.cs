using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformSOS2SetCommand : Command
    {
        public PlatformSOS2SetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformSOS2SetCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.SOS2, commandData)
        {

        }
    }
}
