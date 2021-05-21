using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<LicenseModel> LicenseModels { get; set; }
    }
}
