using Licensing.Manager.ViewModels.WoocommerceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<LicenseModel> LicenseModels { get; set; }
    }

    public class ProductIdViewModel
    {
        public decimal ProductId { get; set; }
    }

    public class ProductResponse
    {
        public ProductViewModel product { get; set; }
        public List<ProductVarientViewModel> varients { get; set; }
        public string Features { get; set; }
        public int licenseType { get; set; }
        public string Durations { get; set; }
    }


    public class VariantIdModel
    {
        public decimal VarientId { get; set; }
    }
    public class ProductCategoryViewModel
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
        public string WcProductId { get; set; }


    }

}
