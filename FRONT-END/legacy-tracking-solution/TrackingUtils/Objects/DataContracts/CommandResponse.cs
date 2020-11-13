using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Enums;

namespace TrackingUtils.Objects.DataContracts
{
    [DataContract]
    public class CommandResponse
    {
        [DataMember]
        public CmdExecStatus Status { get; set; }

        [DataMember]
        public object SuccessData { get; set; }
    }
}
