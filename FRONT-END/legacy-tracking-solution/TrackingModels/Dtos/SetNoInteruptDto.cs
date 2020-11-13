using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetNoInteruptDto
    {
        public string TerminalID { get; set; }
        public List<string> TimePeriods { get; set; }
    }
}
