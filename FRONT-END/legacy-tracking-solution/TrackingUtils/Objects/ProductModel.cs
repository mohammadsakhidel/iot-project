using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects
{
    public class ProductModel
    {
        public string Name { get; set; }
        public string DisplayNameResourceID { get; set; }
        public List<ProductModelVariant> Variants { get; set; }
        public string DisplayName { get; set; }
    }
}
