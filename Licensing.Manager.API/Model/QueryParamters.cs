using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class QueryParamters
    {

    }

    public class ProductConfigurationParameters
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

    public class LicenseQueryParameters
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string LicenseId { get; set; }
        public string Type { get; set; }
        public string Quantity { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Signature { get; set; }
        public string licenseUrl { get; set; }

    }

    public class MachineKeysQueryParamters
    {
        public int Id { get; set; }
        public string LicenseId { get; set; }
        public string MachineKey { get; set; }
        public string Email { get; set; }
    }

    public class RowCount
    {
        public int count { get; set; }
    }
    
    public class CustomerQueryParameters
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }

    public class ProductCategoryParameters
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

    }

    public class LicenseTypeDurationParameter
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public string Duration { get; set; }
    }

    public class ProductQueryParameter
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public decimal regular_price { get; set; }
        public decimal sale_price { get; set; }

        public string sku { get; set; }

        public bool downloadable { get; set; }
        public List<Download> downloads { get; set; }
        public List<Default_Variation_Attributes> attributes { get; set; }

    }

    public class ProductWooCommParameters
    {
        public int Id { get; set; }
        public  int WCProductId  { get;set;}
    }
}
