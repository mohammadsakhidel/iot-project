using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Dtos
{
    public class CustomerDto
    {
        #region Props:
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public DateTime SaveTimeUtc { get; set; }

        public string Marketer { get; set; }
        #endregion

        #region Navigation Props:
        public AspNetUserDto AspNetUser { get; set; }
        #endregion

        #region Data Transfer Needed Fields:
        public string Password { get; set; }
        public string FullName { get; set; }
        #endregion
    }
}
