using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels.WoocommerceModels
{
    public class Woocommerce
    {
        public const string ISO8601BasicDateTimeFormat = "yyyyMMddTHHmmssZ";
        public RestAPI rest;
        public WCObject wc;
        public string BaseURL = "";
        protected string wc_key = "";
        protected string wc_secret = "";

        public Woocommerce(IConfiguration configuration)
        {
            BaseURL = configuration.GetSection("WoocommerceSettings").GetSection("BaseURL").Value;
            wc_key = configuration.GetSection("WoocommerceSettings").GetSection("ConsumerKey").Value;
            wc_secret = configuration.GetSection("WoocommerceSettings").GetSection("ConsumerSecret").Value;
            rest = new RestAPI(BaseURL, wc_key, wc_secret);
            wc = new WCObject(rest);
        }

        public async Task<Product> GetProducts(int id)
        {
            return await wc.Product.Get(id);
        }
        
        public async Task<List<ProductCategory>> GetCategoryByParent(string parent = "0")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (!parms.ContainsKey("parent"))
                parms.Add("parent", parent);

            if (!parms.ContainsKey("per_page"))
                parms.Add("per_page", "100");

            return await wc.ProductCategory.GetAll(parms);
        }

        public async Task<List<ProductCategory>> GetProductCategories(string term)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (!parms.ContainsKey("search"))
                parms.Add("search", term);

            return await wc.ProductCategory.GetAll(parms);
        }

        public async Task<List<ProductVariation>> GetProductVarient(string productId, string term)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (!parms.ContainsKey("search"))
                parms.Add("search", term);

            return await wc.ProductVarient.GetVarient(productId, parms);
        }

        public async Task<object> CreateProduct(object product)
        {
            return await wc.Product.Add(product);
        }

        public async Task<object> UpdateProduct(int id, object product)
        {
            return await wc.Product.Update(id, product);
        }

        public async Task<object> CreateVarient(int productId, object varient)
        {
            return await wc.ProductVarient.AddVarient(productId, varient);
        }

        public async Task<object> UpdateVarient(int productId, int varientId, object varient)
        {
            return await wc.ProductVarient.UpdateVarient(productId, varientId, varient);
        }

        public async Task<object> DeleteVarient(int productId, int varientId)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (!parms.ContainsKey("force"))
                parms.Add("force", "true");

            return await wc.ProductVarient.DeleteVarient(productId, varientId, parms);
        }
    }
}