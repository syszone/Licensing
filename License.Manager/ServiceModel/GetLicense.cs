

//using ServiceStack.ServiceHost;

namespace License.Manager.ServiceModel
{
    //[Route("/licenses/{Id}", "GET, OPTIONS")]
    ////[Route("/licenses/{LicenseId}", "GET, OPTIONS")]
    //[Route("/products/{ProductId}/licenses/{Id}", "GET, OPTIONS")]
    ////[Route("/products/{ProductId}/licenses/{LicenseId}", "GET, OPTIONS")]
    //[Route("/customers/{CustomerId}/licenses/{Id}", "GET, OPTIONS")]
    ////[Route("/customers/{CustomerId}/licenses/{LicenseId}", "GET, OPTIONS")]
    public class GetLicense //: IReturn<LicenseDto>
    {
        public int Id { get; set; }
        //public Guid LicenseId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
    }
}