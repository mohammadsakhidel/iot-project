using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetRemindersDto
    {
        public string TerminalID { get; set; }

        public List<string> Reminders { get; set; }
    }
}
