
 
using License.Manager.Models;
using Microsoft.AspNetCore.Components;
using System;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/customers", "POST, OPTIONS")]
    public class CreateCustomer //: IReturn<Product>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }
}