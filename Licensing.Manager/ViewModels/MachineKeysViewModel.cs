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
        public DateTime? ExpiryDate { get; set; }
        public List<string> Features { get; set; }
    }

}
