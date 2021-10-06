using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class ProductLicenseViewModel
    {
        public int? Id { get; set; }
        public int? WcProductId { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string LicenseURL { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public DateTime? OrderCreatedDate { get; set; }
        public int? WCOrderId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
