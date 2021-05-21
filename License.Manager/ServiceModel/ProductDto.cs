

using System.Collections.Generic;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    public class ProductDto //: IReturn<ProductDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public int NumberOfLicenses { get; set; }
        //public int NumberOfCustomers { get; set; }

        public List<string> ProductFeatures { get; set; }
    }
}