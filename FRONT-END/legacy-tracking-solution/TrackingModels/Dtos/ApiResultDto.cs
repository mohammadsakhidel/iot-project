using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class ApiResultDto
    {
        public bool Succeeded { get; set; }
        public string Data { get; set; }
        public int ErrorCode { get; set; }
    }
}
