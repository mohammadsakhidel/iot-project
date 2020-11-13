using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Constants;

namespace TrackingUtils.Objects.Commands
{
    public class BidirectionalSendVoiceCommand : Command
    {
        #region Ctors:
        public BidirectionalSendVoiceCommand(string manufacturer, string deviceId, int contentLength, string commandId, string commandData)
            : base(manufacturer, deviceId, contentLength, commandId, commandData)
        {
        }

        public BidirectionalSendVoiceCommand(string terminalId, string commandData)
            : base(terminalId, CommandTypes.TK, commandData)
        {

        }
        #endregion

        #region Methods:
        public override Command GenerateReply()
        {
            var reply = new BidirectionalSendVoiceCommand(Manufacturer, DeviceID, 4, CommandTypes.TK, "1");
            return reply;
        }
        #endregion
    }
}
