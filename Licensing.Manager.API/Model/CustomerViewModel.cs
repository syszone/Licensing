using System;

namespace Licensing.Manager.API.Model
{
    public class CustomerViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
    }

    public class CustomerResponseViewModel
    {
        public int LicenseType { get; set; }
    }

}
