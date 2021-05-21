

using Licensing;
using System;
using System.Collections.Generic;
//using Portable.Licensing;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/licenses", "POST, OPTIONS")]
    //[Route("/products/{ProductId}/licenses", "POST, OPTIONS")]
    //[Route("/customers/{CustomerId}/licenses", "POST, OPTIONS")]
    public class CreateLicense //: IReturn<LicenseDto>
    {
        public Guid LicenseId { get; set; }
        public LicenseType LicenseType { get; set; }
        public int Quantity { get; set; }
        public DateTime Expiration { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public Dictionary<string, string> ProductFeatures { get; set; }
        public Dictionary<string, string> AdditionalAttributes { get; set; }
    }
}