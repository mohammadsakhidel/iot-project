using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Enums;

namespace TrackingUtils.Objects.Exceptions
{
    public class CommandFailedException : Exception
    {
        public CommandFailedException(CmdExecStatus stat)
        {
            ExecutionStatus = stat;
        }

        public CmdExecStatus ExecutionStatus { get; set; }
    }
}
