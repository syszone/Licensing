using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
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
        public List<ProductDownloadLine> downloadsfile { get; set; }
        public List<Category> categories { get; set; }
        public bool downloadable { get; set; }
    }

    public class Category
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    [DataContract]
    public class ProductDownloadLine
    {
        public string id { get; set; }
        public string name { get; set; }
        public string file { get; set; }

    }


}
