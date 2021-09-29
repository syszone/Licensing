using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class OrderViewModel
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string order_key { get; set; }
        public DateTime date_created { get; set; }
        public Address billing { get; set; }
        public Address shipping { get; set; }
        public string payment_method { get; set; }
        public string payment_method_title { get; set; }
        public string transaction_id { get; set; }
        public List<Line_Items> line_items { get; set; }
    }

    public class Address
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class Line_Items
    {
        public int id { get; set; }
        public string name { get; set; }
        public int product_id { get; set; }
        public int variation_id { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
        public int price { get; set; }
        public List<LicenseDurationViewModel> meta_data { get; set; }
    }

}
