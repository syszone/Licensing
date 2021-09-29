using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels.WoocommerceModels
{
    public class ProductVariationsViewModel
    {
        public DateTime date_created { get; set; }
        public string description { get; set; }
        public string sku { get; set; }
        public string price { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }
        public bool status { get; set; }
        public bool purchasable { get; set; }
        public bool _virtual { get; set; }
        public bool downloadable { get; set; }
        public object[] downloads { get; set; }
        public int download_limit { get; set; }
        public int download_expiry { get; set; }
        public List<Attribute> attributes { get; set; }
    }

    //public class Rootobject
    //{
        
    //}

    public class Attribute
    {
        public int id { get; set; }
        public string name { get; set; }
        public string option { get; set; }
    }

}
