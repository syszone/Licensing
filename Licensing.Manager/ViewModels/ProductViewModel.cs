
using System;
using System.Collections.Generic;


namespace Licensing.Manager.Models
{
    public class ProductViewModel 
    {
        public Guid ProductId { get; set; }         
        public string Name { get; set; }         
        public string Description { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
        public List<LicenseModel> LicenseModels { get; set; }
    }
}
