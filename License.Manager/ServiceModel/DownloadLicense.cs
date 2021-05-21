
using System;
//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/licenses/issue", "GET, OPTIONS")]
    public class DownloadLicense
    {
        public Guid Token { get; set; }
    }
}