
using License.Manager.Models;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/customers/{Id}", "GET, OPTIONS")]
    public class GetCustomer //: IReturn<Customer>
    {
        public int Id { get; set; }
    }
}