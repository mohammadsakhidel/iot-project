using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class SetPhoneBookContactsDto
    {
        public string TerminalID { get; set; }
        public KeyValuePair<string, string>[] Contacts { get; set; }
    }
}
