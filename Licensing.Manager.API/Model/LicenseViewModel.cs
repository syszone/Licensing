using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class LicenseViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string LicenseId { get; set; }
        public string Type { get; set; }
        public string Quantity { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Signature { get; set; }
        public string licenseUrl { get; set; }
    }
}
