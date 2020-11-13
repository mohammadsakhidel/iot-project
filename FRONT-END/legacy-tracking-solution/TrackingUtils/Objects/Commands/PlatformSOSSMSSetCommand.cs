using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformSOSSMSSetCommand : Command
    {
        public PlatformSOSSMSSetCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformSOSSMSSetCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.SOSSMS, commandData)
        {

        }
    }
}
