using Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Models
{
    public class LicenseModel 
    {
        public Guid LicenseModelId { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Date Expiry Term")]
        public int ExpiryTerm { get; set; }
        public KeyPair KeyPair { get; set; }
        public LicenseType LicenseType { get; set; }
        public ICollection<LicenseModelFeature> LicenseModelFeatures { get; set; }
        public virtual Product Product { get; set; }
    }
}
