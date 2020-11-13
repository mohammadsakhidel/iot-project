using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Objects;

namespace TrackingUtils.Constants
{
    public class ProductBag
    {
        public static List<Product> Products
        {
            get
            {
                var products = new List<Product> {
                    #region kidswatch product:
                    new Product {
                        Name = "kidswatch",
                        DisplayNameResourceID = "product_kids_watch",
                        Models = new List<ProductModel> {
                            #region Q50 model:
                            new ProductModel {
                                Name = "Q50",
                                DisplayNameResourceID = "product_model_q50",
                                Variants = new List<ProductModelVariant> {
                                    new ProductModelVariant { Name = "pink", DisplayNameResourceID = "product_model_variant_pink" },
                                    new ProductModelVariant { Name = "blue", DisplayNameResourceID = "product_model_variant_blue" },
                                    new ProductModelVariant { Name = "black", DisplayNameResourceID = "product_model_variant_black" },
                                    new ProductModelVariant { Name = "yellow", DisplayNameResourceID = "product_model_variant_yellow" }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                };
                return products;
            }
        }
    }
}
