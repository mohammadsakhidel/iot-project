using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Enums
{
    [DataContract]
    public enum CmdExecStatus
    {
        [EnumMember]
        succeeded = 1,

        [EnumMember]
        failed_terminal_disconnected = 2,

        [EnumMember]
        failed_terminal_no_response = 3
    }
}
