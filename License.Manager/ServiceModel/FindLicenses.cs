

using System;
using System.Collections.Generic;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
   /// [Route("/licenses", "GET, OPTIONS")]
   // [Route("/products/{ProductId}/licenses", "GET, OPTIONS")]
   // [Route("/customers/{CustomerId}/licenses", "GET, OPTIONS")]
    //[Route("/licenses/page/{Page}")]
    public class FindLicenses //: IReturn<List<LicenseDto>>
    {
        //public LicenseType? LicenseType { get; set; }
        //public DateTime? Expiration { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }

        //public int? Page { get; set; }
        //public int? PageSize { get; set; }
    }
}