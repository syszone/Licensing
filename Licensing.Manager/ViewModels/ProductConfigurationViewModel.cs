using Licensing.Manager.ViewModels.WoocommerceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class ProductConfigurationViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int LicenseId { get; set; }
        public string DurationId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string SKU { get; set; }
        public List<ProductDownloadLine> downloadsdata { get; set; }
        public List<Download> downloadsfile { get; set; }

        public List<ProductVarientViewModel> Variants { get; set; }

        public List<Category> categories { get; set; }
        public bool downloadable { get; set; }

        public string Features { get; set; }
        public List<Meta_Data> meta_data { get; set; }
    }
}
