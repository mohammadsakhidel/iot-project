using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Objects
{
    public class Product
    {
        public string Name { get; set; }
        public string DisplayNameResourceID { get; set; }
        public List<ProductModel> Models { get; set; }
        public string DisplayName { get; set; }
    }
}
