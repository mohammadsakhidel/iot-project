using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetServerDto
    {
        public string TerminalID { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }
    }
}
