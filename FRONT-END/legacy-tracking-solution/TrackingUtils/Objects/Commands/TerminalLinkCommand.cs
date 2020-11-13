using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class TerminalLinkCommand : Command
    {
        #region Ctors:
        public TerminalLinkCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData)
            : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }
        #endregion

        #region Methods:
        public override Command GenerateReply()
        {
            var reply = new TerminalLinkCommand(Manufacturer, DeviceID, 2, CommandTypes.LK, "");
            return reply;
        }
        #endregion
    }
}
