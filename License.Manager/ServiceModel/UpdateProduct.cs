

using System.Collections.Generic;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/products/{Id}", "PUT, DELETE, OPTIONS")]
    public class UpdateProduct //: IReturn<ProductDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ProductFeatures { get; set; }
    }
}