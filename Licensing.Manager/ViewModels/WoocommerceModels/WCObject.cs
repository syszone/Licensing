using System;
using System.Runtime.Serialization;

namespace Licensing.Manager.ViewModels.WoocommerceModels
{
    public class WCObject<T1, T2, T3> where T1 : Product where T2 : ProductCategory where T3 : ProductVariation
    {
        protected RestAPI API { get; set; }
        public static Func<string, object, object> MetaValueProcessor { get; set; }
        public static Func<string, object, object> MetaDisplayValueProcessor { get; set; }

        public WCObject(RestAPI api)
        {
            if (api.Version != APIVersion.Version3 && api.Version != APIVersion.ThirdPartyPlugins)
                throw new Exception("Please use WooCommerce Restful API Version 3 url for this WCObject. e.g.: http://www.yourstore.co.nz/wp-json/wc/v3/");

            API = api;

            Product = new WCProductItem(api);
            ProductCategory = new WCProductCategoryItem(api);
            ProductVarient = new WCProductVarientItem(api);
        }

        public WCProductItem Product { get; protected set; }
        public WCProductCategoryItem ProductCategory { get; protected set; }
        public WCProductVarientItem ProductVarient { get; protected set; }


        public class WCProductItem : WCItem<T1>
        {
            public WCProductItem(RestAPI api) : base(api)
            {
                API = api;
            }
        }
        public class WCProductCategoryItem : WCItem<T2>
        {
            public WCProductCategoryItem(RestAPI api) : base(api)
            {
                API = api;
            }
        }
        public class WCProductVarientItem : WCItem<T3>
        {
            public WCProductVarientItem(RestAPI api) : base(api)
            {
                API = api;
            }
        }


        [DataContract]
        public class MetaData
        {
            /// <summary>
            /// Meta ID. 
            /// read-only
            /// </summary>
            [DataMember(EmitDefaultValue = false)]
            public uint? id { get; set; }

            /// <summary>
            /// Meta key.
            /// </summary>
            [DataMember(EmitDefaultValue = false)]
            public string key { get; set; }

            /// <summary>
            /// Meta value.
            /// </summary>
            private object preValue;
            [DataMember(EmitDefaultValue = false)]
            public object value
            {
                get
                {
                    return preValue;
                }
                set
                {
                    if (MetaValueProcessor != null)
                        preValue = MetaValueProcessor.Invoke(GetType().Name, value);
                    else
                        preValue = value;
                }
            }

            /// <summary>
            /// Display key.
            /// </summary>
            [DataMember(EmitDefaultValue = false)]
            public string display_key { get; set; }

            /// <summary>
            /// Display value.
            /// </summary>
            private object preDisplayValue;
            [DataMember(EmitDefaultValue = false)]
            public object display_value
            {
                get => preDisplayValue;
                set
                {
                    if (MetaDisplayValueProcessor != null)
                        preDisplayValue = MetaDisplayValueProcessor.Invoke(GetType().Name, value);
                    else
                        preDisplayValue = value;
                }
            }
        }
    }

    public class WCObject : WCObject<Product, ProductCategory, ProductVariation>
    {
        public WCObject(RestAPI api) : base(api)
        {

        }
    }
}