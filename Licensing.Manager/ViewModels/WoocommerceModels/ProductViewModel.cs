using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licensing.Manager.ViewModels.WoocommerceModels
{

    public class ProductViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_created_gmt { get; set; }
        public DateTime date_modified { get; set; }
        public DateTime date_modified_gmt { get; set; }
        public string type { get; set; } = "variable";
        public string status { get; set; } = "publish";
        public bool featured { get; set; } = false;
        public string catalog_visibility { get; set; } = "visible";
        public string description { get; set; }
        public string short_description { get; set; }
        public string sku { get; set; }
        public string price { get; set; }
        public string regular_price { get; set; } = "0";
        public string sale_price { get; set; } = "0";
        public DateTime date_on_sale_from { get; set; }
        public DateTime date_on_sale_from_gmt { get; set; }
        public DateTime date_on_sale_to { get; set; }
        public DateTime date_on_sale_to_gmt { get; set; }
        public bool on_sale { get; set; } = false;
        public bool purchasable { get; set; } = true;
        public int total_sales { get; set; } = 0;
        public bool downloadable { get; set; }
        public List<Download> downloads { get; set; }
        public int download_limit { get; set; } = -1;
        public int download_expiry { get; set; } = -1;
        public string external_url { get; set; } = "";
        public string button_text { get; set; } = "";
        public string tax_status { get; set; } = "taxable";
        public string tax_class { get; set; } = "Standard";
        public bool manage_stock { get; set; } = false;
        public int stock_quantity { get; set; }
        public string backorders { get; set; }
        public bool backorders_allowed { get; set; } = false;
        public bool backordered { get; set; }
        public decimal low_stock_amount { get; set; } = 0;
        public bool sold_individually { get; set; } = false;

        public List<int> related_ids { get; set; }
        public bool shipping_required { get; set; } = false;
        public bool shipping_taxable { get; set; } = false;
        public string shipping_class { get; set; } = "UnAssemble";
        public int shipping_class_id { get; set; } = 0;
        public bool reviews_allowed { get; set; } = false;
        public string average_rating { get; set; } = "0";
        public int rating_count { get; set; } = 0;
        public List<string> upsell_ids { get; set; }
        public List<string> cross_sell_ids { get; set; }
        public int parent_id { get; set; } = 0;
        public string purchase_note { get; set; }
        public List<Category> categories { get; set; }
        public List<ProductImages> images { get; set; }
        public int menu_order { get; set; } = 0;
        public string stock_status { get; set; } = "instock";
        public List<string> variations { get; set; }
        public List<Variation_Attributes> attributes { get; set; }
        public List<Default_Variation_Attributes> default_attributes { get; set; }
        public List<Meta_Data> meta_data { get; set; }



    }
    public class LicenseDuration
    {
        public List<string> durationid { get; set; }

        public int licensetype { get; set; }
    }

    public class Features
    {
        public List<string> FeaturesId { get; set; }
    }

    public class Download
    {
        public string name { get; set; }
        public string file { get; set; }
    }
    public class ProductImages
    {
        public string src { get; set; }
    }
    public class Dimensions
    {
        public string length { get; set; } = "";
        public string width { get; set; } = "";
        public string height { get; set; } = "";
    }

    public class _Links
    {
        public List<Self> self { get; set; }
        public List<Collection> collection { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class Category
    {
        public int? id { get; set; }
        public string name { get; set; }
    }

    public class Meta_Data
    {
        public string key { get; set; }
        public List<Meta_Data_Value> value { get; set; }
    }

    public class Meta_Data_Value
    {
        public string title { get; set; }
        public string id { get; set; }
        public string content { get; set; }
    }


    public class Composite_Items
    {
        public string id { get; set; } = "";
        public string title { get; set; }
        public string description { get; set; } = "";
        public string query_type { get; set; } = "product_ids";
        public List<int> query_ids { get; set; }
        public int default_option_id { get; set; } = 0;
        public int thumbnail_id { get; set; } = 0;
        public string thumbnail_src { get; set; } = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
        public int quantity_min { get; set; }
        public int quantity_max { get; set; }
        public bool priced_individually { get; set; }
        public bool shipped_individually { get; set; }
        public bool optional { get; set; }
        public string discount { get; set; }
        public string options_style { get; set; } = "dropdowns";
        public string pagination_style { get; set; } = "classic";
        public string display_prices { get; set; } = "absolute";
        public bool show_sorting_options { get; set; } = false;
        //public object attribute_filter_ids { get; set; }
        public bool show_filtering_options { get; set; } = false;
        public bool product_title_visible { get; set; } = true;
        public bool product_descr_visible { get; set; } = true;
        public bool product_price_visible { get; set; } = true;
        public bool product_thumb_visible { get; set; } = true;
        public bool subtotal_visible_product { get; set; } = true;
        public bool subtotal_visible_cart { get; set; } = true;
        public bool subtotal_visible_orders { get; set; } = true;
    }

    public class Bundled_Items
    {
        public int bundled_item_id { get; set; }
        public int product_id { get; set; } = 0;
        public int menu_order { get; set; } = 0;
        public int quantity_min { get; set; } = 0;
        public int quantity_max { get; set; } = 0;
        public int quantity_default { get; set; } = 0;
        public bool priced_individually { get; set; } = false;
        public bool shipped_individually { get; set; } = false;
        public bool override_title { get; set; } = false;
        public string title { get; set; }
        public bool override_description { get; set; } = false;
        public string description { get; set; } = "";
        public bool optional { get; set; } = false;
        public bool hide_thumbnail { get; set; } = false;
        public string discount { get; set; } = "0";
        public bool override_variations { get; set; } = false;
        public bool override_default_variation_attributes { get; set; } = false;
        public string single_product_visibility { get; set; } = "hidden";
        public string cart_visibility { get; set; } = "hidden";
        public string order_visibility { get; set; } = "hidden";
        public string single_product_price_visibility { get; set; } = "visible";
        public string cart_price_visibility { get; set; } = "visible";
        public string order_price_visibility { get; set; } = "visible";
        public string stock_status { get; set; } = "in_stock";
    }

    public class Variation_Attributes
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> options { get; set; }

        public bool variation { get; set; } = true;
        public bool visible { get; set; } = true;

    }

    public class Default_Variation_Attributes
    {

        public string name { get; set; }
        public string options { get; set; }

    }


    public class JSTree
    {
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public JSTreeAttr li_attr { get; set; }
        public JSTree[] children { get; set; }
        public State state { get; set; }
    }
    public class JSTreeAttr
    {
        public string id;
        public bool selected;
    }
    public class State
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class ProductWoocommViewModel
    {

        public int Id { get; set; }
        public int WCProductId { get; set; }
        public int LicenseDurationId { get; set; }
    }


    public class VarientWoocommViewModel
    {

        public int Id { get; set; }
        public int WCVarientId { get; set; }
    }
}