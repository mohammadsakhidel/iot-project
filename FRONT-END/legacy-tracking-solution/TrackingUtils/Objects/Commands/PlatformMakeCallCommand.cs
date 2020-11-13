using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class PlatformMakeCallCommand : Command
    {
        #region Ctors:
        public PlatformMakeCallCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData) : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public PlatformMakeCallCommand(string terminalId, string phoneNumber) 
            : base(terminalId, CommandTypes.CALL, phoneNumber)
        {
        }
        #endregion
    }
}
