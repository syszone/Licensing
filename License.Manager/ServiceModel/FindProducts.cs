
using System.Collections.Generic;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/products", "GET, OPTIONS")]
    //[Route("/products/page/{Page}")]
    public class FindProducts //: IReturn<List<ProductDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //public int? Page { get; set; }
        //public int? PageSize { get; set; }
    }
}