using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class ProductLicenseViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string LicenseType { get; set; }
        public string LicenseFileUrl { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
    }
}
