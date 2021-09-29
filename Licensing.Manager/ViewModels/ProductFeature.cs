using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licensing.Manager.ViewModels
{
    public class ProductFeature
    {
        public Guid FeatureId { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual Product Product { get; set; }
    }
}
