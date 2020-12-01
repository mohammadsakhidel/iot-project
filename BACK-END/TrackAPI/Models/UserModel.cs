using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TrackLib.Constants;

namespace TrackAPI.Models {
    public class UserModel {

        public string Id { get; set; }

        [Required]
        [RegularExpression(Patterns.NAME)]
        public string GivenName { get; set; }

        [Required]
        [RegularExpression(Patterns.NAME)]
        public string Surname { get; set; }

        [RegularExpression(Patterns.NAME)]
        public string State { get; set; }

        [RegularExpression(Patterns.NAME)]
        public string City { get; set; }

        public string Address { get; set; }

        [RegularExpression(Patterns.PHONE)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(Patterns.EMAIL)]
        public string Email { get; set; }

        [RegularExpression(Patterns.PASSWORD)]
        public string Password { get; set; }

        public bool? IsActive { get; set; }

        public string CreationTime { get; set; }

    }
}
