


using License.Manager.Models;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/customers/{Id}", "PUT, DELETE, OPTIONS")]
    public class UpdateCustomer //: IReturn<Customer>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }
}