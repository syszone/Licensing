using System;

namespace Licensing.Manager.ViewModels
{
    public class CustomerViewModel
    {
        public int WCOrderId { get; set; }
        public int WCProductId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string productLink { get; set; }
        public int LicenseDurationId { get; set; }
    }

    public class CustomerResponseViewModel
    {
        public int LicenseType { get; set; }
        public int CustomerId { get; set; }
        public int WCOrderId { get; set; }
    }

    public class CustomerIdClass
    {
        public int CustomerId { get; set; }
    }
}
