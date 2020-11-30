using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.DTOs {
    public class UserDto {
        public string Id { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string CreationTime { get; set; }
    }
}
