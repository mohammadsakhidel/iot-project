using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformSOS3SetCommand : Command
    {
        public PlatformSOS3SetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformSOS3SetCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.SOS3, commandData)
        {

        }
    }
}
