using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Constants
{
    public class Patterns
    {
        public const string terminal_command = @"^\[[A-Za-z0-9]{2}\*[0-9]+\*[0-9A-Fa-f]{4}\*[A-Za-z0-9]{2,}(,.+)?\]$";
        public const string terminal_command_list = @"^(\[[A-Za-z0-9]{2}\*[0-9]+\*[0-9A-Fa-f]{4}\*[A-Za-z0-9]{2,}(,.+)?\])+$";
        public const string terminal_command_find = @"\[[A-Z0-9a-z]{2}\*[0-9]+\*[0-9A-Fa-f]{4}\*[A-Za-z0-9]{2,}(,.+)?\]";
        public const string phonenumber = @"^((\+|00)\d{1,3}|0)\d{10}$";
        public const string timeperiod = @"^\d{2}:\d{2}-\d{2}:\d{2}$";
        public const string timeperiods = @"^(\d{2}:\d{2}-\d{2}:\d{2})(,\d{2}:\d{2}-\d{2}:\d{2})*$";
        public const string reminder = @"^\d{2}:\d{2}-\d-\d(-\d{7})?$";
        public const string number = @"^\d+$";
    }
}
