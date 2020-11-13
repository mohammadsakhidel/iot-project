using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Enums
{
    [Flags]
    public enum ApiErrorCode
    {
        invalid_username = 1,
        account_blocked = 2,
        invalid_password = 4


    }
}
