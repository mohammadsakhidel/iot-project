using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Models {
    public class ContactModel {

        [Required]
        public string Number { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
