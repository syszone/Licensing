using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels.WoocommerceModels
{
    public class ProductVarientViewModel
    {
        public int? id { get; set; }
        public DateTime? date_created { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }
        public string status { get; set; } = "publish";
        public bool on_sale { get; set; }
        public bool purchasable { get; set; } = true;
        public bool downloadable { get; set; } = false;
        public List<Download> downloads { get; set; }
        public int download_limit { get; set; }
        public int download_expiry { get; set; }
        public List<ProductAttributes> attributes { get; set; }
        //public string menu_order { get; set; } = "1";
    }

    public class ProductAttributes
    {
        public string id { get; set; } = "0";
        public string name { get; set; }
        public string option { get; set; }

    }

    public class GetProductVarientResult
    {
        public int Id { get; set; }
        public int WCVarientId { get; set; }
        public int ProductId { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
        public int WCAttributeId { get; set; }
        public decimal? RegularPrice { get; set; }
        public decimal? SalesPrice { get; set; }

        public string Description { get; set; }

    }

    public class GetProductVarientImageResult
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }

    }

    public class GetProductCategoryResult
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }


}
