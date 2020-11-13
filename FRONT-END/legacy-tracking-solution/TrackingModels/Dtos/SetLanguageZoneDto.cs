using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetLanguageZoneDto
    {
        public string TerminalID { get; set; }

        public string Language { get; set; }

        public string TimeZone { get; set; }
    }
}
