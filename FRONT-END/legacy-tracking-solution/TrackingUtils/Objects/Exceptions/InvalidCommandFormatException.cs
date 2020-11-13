using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects.Exceptions
{
    public class InvalidCommandFormatException : Exception
    {
        public InvalidCommandFormatException(string message) : base(message)
        {

        }
    }
}
