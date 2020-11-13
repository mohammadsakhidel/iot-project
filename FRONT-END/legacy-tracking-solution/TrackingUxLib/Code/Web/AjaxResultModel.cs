using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUxLib.Code.Web
{
    public class AjaxResultModel
    {
        public bool Done { get; set; } = true;
        public string Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
