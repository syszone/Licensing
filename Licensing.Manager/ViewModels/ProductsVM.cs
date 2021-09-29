using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class ProductsVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public string sku { get; set; }
        public string price { get; set; }
        public string regular_price { get; set; } = "0";
        public string sale_price { get; set; } = "0";
        public string durations { get; set; }
        public string licensetype { get; set; }
        public string wcProductId { get; set; }
    }
}
