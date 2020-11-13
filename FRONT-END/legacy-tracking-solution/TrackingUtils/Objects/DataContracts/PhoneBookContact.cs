using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects.DataContracts
{
    [DataContract]
    public class PhoneBookContact
    {
        #region Props:
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Number { get; set; }
        #endregion

        #region Overrides:
        public override string ToString()
        {
            return $"{Number},{Name}";
        }
        #endregion
    }
}
