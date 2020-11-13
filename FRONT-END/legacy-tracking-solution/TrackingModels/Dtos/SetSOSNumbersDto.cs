using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetSOSNumbersDto
    {
        public string TerminalID { get; set; }
        public string SOS1 { get; set; }
        public string SOS2 { get; set; }
        public string SOS3 { get; set; }
    }
}
