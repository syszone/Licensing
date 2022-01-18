using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class MachineKeysViewModel
    {
        public License license { get; set; }
        public string MachineKey { get; set; }
        public string Email { get; set; }
    }

    public class VerifyResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public DateTime? RegisterdDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public List<string> Features { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string LicenseType { get; set; }
        public string LicenseTypeDuration { get; set; }
        public string ProductLink { get; set; }

    }

}
