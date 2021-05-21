
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/products/{Id}", "GET, OPTIONS")]
    public class GetProduct //: IReturn<ProductDto>
    {
        public int Id { get; set; }
    }
}