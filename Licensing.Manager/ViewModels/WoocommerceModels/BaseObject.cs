using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels.WoocommerceModels
{
    [DataContract]
    public class JsonObject
    {
        [OnSerializing]
        void OnSerializing(StreamingContext ctx)
        {
            foreach (PropertyInfo pi in GetType().GetRuntimeProperties())
            {
                PropertyInfo objValue = GetType().GetRuntimeProperties().FindByName(pi.Name + "Value");
                if (objValue != null && pi.GetValue(this) != null)
                {
                    if (pi.PropertyType == typeof(decimal?))
                    {
                        if (GetType().FullName.StartsWith("Amazon_SP_API.Woocommerce") ||
                            GetType().GetTypeInfo().BaseType.FullName.StartsWith("Amazon_SP_API.Woocommerce"))
                            objValue.SetValue(this, (pi.GetValue(this) as decimal?).Value.ToString(CultureInfo.InvariantCulture));
                        else
                            objValue.SetValue(this, decimal.Parse(pi.GetValue(this).ToString(), CultureInfo.InvariantCulture));
                    }
                    else if (pi.PropertyType == typeof(int?))
                    {
                        objValue.SetValue(this, int.Parse(pi.GetValue(this).ToString(), CultureInfo.InvariantCulture));
                    }
                    else if (pi.PropertyType == typeof(DateTime?))
                    {
                        objValue.SetValue(this, ((DateTime?)pi.GetValue(this)).Value.ToString("yyyy-MM-ddTHH:mm:ss"));
                    }
                }
            }
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext ctx)
        {
            foreach (PropertyInfo pi in GetType().GetRuntimeProperties())
            {
                PropertyInfo objValue = GetType().GetRuntimeProperties().FindByName(pi.Name + "Value");

                if (objValue != null)
                {
                    if (pi.PropertyType == typeof(decimal?))
                    {
                        object value = objValue.GetValue(this);

                        if (!(value == null || value.ToString() == string.Empty))
                            pi.SetValue(this, decimal.Parse(value.ToString(), CultureInfo.InvariantCulture));
                    }
                    else if (pi.PropertyType == typeof(int?))
                    {
                        object value = objValue.GetValue(this);

                        if (!(value == null || value.ToString() == string.Empty))
                            pi.SetValue(this, int.Parse(value.ToString(), CultureInfo.InvariantCulture));
                    }
                    else if (pi.PropertyType == typeof(DateTime?))
                    {
                        object value = objValue.GetValue(this);

                        if (!(value == null || value.ToString() == string.Empty))
                            pi.SetValue(this, DateTime.Parse(value.ToString()));
                    }
                }
            }
        }


        //[OnDeserializing]
        //void tset(StreamingContext ctx)
        //{
        //    if (GetType().Name.Contains("ProductMeta"))
        //        foreach (PropertyInfo pi in GetType().GetRuntimeProperties())
        //        {

        //        }
        //}
    }

    //public class MyCustomerResolver : DataContractResolver
    //{
    //    public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
    //    {
    //        if (dataContractType == typeof(string))
    //        {
    //            XmlDictionary dictionary = new XmlDictionary();
    //            typeName = dictionary.Add("SomeCustomer");
    //            typeNamespace = dictionary.Add("http://tempuri.com");
    //            return true;
    //        }
    //        else
    //        {
    //            return knownTypeResolver.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace);
    //        }
    //    }

    //    public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class BatchObject<T>
    {
        [DataMember(EmitDefaultValue = false)]
        public List<T> create { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<T> update { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<int> delete { get; set; }

        [IgnoreDataMember]
        public List<T> DeletedItems { get; set; }
    }

    public class WCItem<T>
    {
        public string APIEndpoint { get; protected set; }
        public RestAPI API { get; protected set; }

        public WCItem(RestAPI api)
        {
            API = api;
            if (typeof(T).BaseType.GetRuntimeProperty("Endpoint") == null)
                APIEndpoint = typeof(T).GetRuntimeProperty("Endpoint").GetValue(null).ToString();
            else
                APIEndpoint = typeof(T).BaseType.GetRuntimeProperty("Endpoint").GetValue(null).ToString();
        }
        public virtual async Task<List<T>> GetVarient(object productId, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<List<T>>(await API.GetRestful(APIEndpoint.Replace("${productId}", productId.ToString())).ConfigureAwait(false));
        }

        public virtual async Task<T> AddVarient(object productId, object item, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<T>(await API.PostRestful(APIEndpoint.Replace("${productId}", productId.ToString()), item, parms).ConfigureAwait(false));
        }

        public virtual async Task<List<T>> UpdateVarient(object productId, object varientId, object item, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<List<T>>(await API.PutRestful(APIEndpoint.Replace("${productId}", productId.ToString()) + "/" + varientId, item).ConfigureAwait(false));
        }

        public virtual async Task<List<T>> DeleteVarient(object productId, object varientId, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<List<T>>(await API.DeleteRestful(APIEndpoint.Replace("${productId}", productId.ToString()) + "/" + varientId, parms).ConfigureAwait(false));
        }

        public virtual async Task<T> Get(int id, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<T>(await API.GetRestful(APIEndpoint + "/" + id.ToString(), parms).ConfigureAwait(false));
        }
        public virtual async Task<List<T>> GetAll(Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<List<T>>(await API.GetRestful(APIEndpoint, parms).ConfigureAwait(false));
        }
        public virtual async Task<T> Add(object item, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<T>(await API.PostRestful(APIEndpoint, item, parms).ConfigureAwait(false));
        }
        public virtual async Task<T> Update(int id, object item, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<T>(await API.PutRestful(APIEndpoint + "/" + id.ToString(), item, parms).ConfigureAwait(false));
        }
    }

    public class WCSubItem<T>
    {
        public string APIEndpoint { get; protected set; }
        public string APIParentEndpoint { get; protected set; }
        public RestAPI API { get; protected set; }

        public WCSubItem(RestAPI api, string parentEndpoint)
        {
            API = api;
            if (typeof(T).BaseType.FullName.Contains("v2"))
                APIEndpoint = typeof(T).BaseType.GetRuntimeProperty("Endpoint").GetValue(null).ToString();
            else
                APIEndpoint = typeof(T).GetRuntimeProperty("Endpoint").GetValue(null).ToString();

            APIParentEndpoint = parentEndpoint;
        }

        public virtual async Task<T> Get(int id, int parentId, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<T>(await API.GetRestful(APIParentEndpoint + "/" + parentId.ToString() + "/" + APIEndpoint + "/" + id.ToString(), parms).ConfigureAwait(false));
        }

        public virtual async Task<List<T>> GetAll(object parentId, Dictionary<string, string> parms = null)
        {
            return API.DeserializeJSon<List<T>>(await API.GetRestful(APIParentEndpoint + "/" + parentId.ToString() + "/" + APIEndpoint, parms).ConfigureAwait(false));
        }

    }
}