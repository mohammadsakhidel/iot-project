using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetUploadIntervalDto
    {
        public string TerminalID { get; set; }

        public int IntervalSeconds { get; set; }
    }
}
