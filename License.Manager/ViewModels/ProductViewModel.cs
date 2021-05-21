using Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace License.Manager.Models
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
