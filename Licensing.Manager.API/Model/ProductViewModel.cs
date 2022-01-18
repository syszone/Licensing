using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class ProductViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }

        public string sku { get; set; }

        public bool downloadable { get; set; }
        public List<Download> downloads { get; set; }
        public List<string> variations { get; set; }
        public List<Default_Variation_Attributes> attributes { get; set; }
    }
    public class Download
    {
        public string name { get; set; }
        public string file { get; set; }
    }
    public class ProductWoocommViewModel
    {
        public int Id { get; set; }
        public int WCProductId { get; set; }


    }
    public class Default_Variation_Attributes
    {
        
        public string name { get; set; }
        public List<string> options { get; set; }

        public bool variation { get; set; } = false;
        public bool visible { get; set; } = true;

    }

}
