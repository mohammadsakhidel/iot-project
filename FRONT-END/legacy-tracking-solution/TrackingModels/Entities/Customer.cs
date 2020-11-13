using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingModels.Entities
{
    public class Customer
    {
        #region Props:
        public int ID { get; set; }

        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(512)]
        public string Address { get; set; }

        [MaxLength(16)]
        public string Province { get; set; }

        [MaxLength(16)]
        public string City { get; set; }

        [Required]
        public DateTime SaveTimeUtc { get; set; }

        [MaxLength(16)]
        public string Marketer { get; set; }
        #endregion

        #region Navigation Props:
        public virtual ICollection<Terminal> Terminals { get; set; }
        #endregion
    }
}
