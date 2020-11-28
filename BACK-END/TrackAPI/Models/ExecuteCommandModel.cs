using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class ExecuteCommandModel {

        [Required]
        public string TrackerId { get; set; }

        [Required]
        public string CommandType { get; set; }

        public string Payload { get; set; }
    }
}
