

using System;
using System.Collections.Generic;
 
using License.Manager.Models;
using Licensing;

namespace License.Manager.ServiceModel
{
    public class LicenseDto
    {
        public int Id { get; set; }
        public Guid LicenseId { get; set; }
        public LicenseType LicenseType { get; set; }
        public int Quantity { get; set; }
        public DateTime Expiration { get; set; }
        public License.Manager.Models.Customer Customer { get; set; }
        public ProductDto Product { get; set; }
        public Dictionary<string, string> ProductFeatures { get; set; }
        public Dictionary<string, string> AdditionalAttributes { get; set; }
    }
}
