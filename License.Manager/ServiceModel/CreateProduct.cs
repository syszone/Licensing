

using System;
using System.Collections.Generic;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/products", "POST, OPTIONS")]
    public class CreateProduct //: IReturn<ProductDto>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ProductFeatures { get; set; }
    }
}