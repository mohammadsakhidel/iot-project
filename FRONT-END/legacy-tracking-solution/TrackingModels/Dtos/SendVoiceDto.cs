using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SendVoiceDto
    {
        public string TerminalID { get; set; }

        public string WavBase64 { get; set; }
    }
}
