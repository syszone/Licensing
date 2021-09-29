
using System;


namespace Licensing.Manager.ViewModels
{
    public partial class LicenseData
    {
        public Guid LicenseId { get; set; }
        public LicenseType LicenseType { get; set; }
        public int Quantity { get; set; }
        public int QuantityUtilized { get; set; } 
        public DateTime Expiration { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductFeatures { get; set; }
        public string AdditionalAttributes { get; set; }
        
    }
}
