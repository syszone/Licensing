using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class LicenseTypeDurationViewModel
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public string Duration { get; set; }
        public int WCAttributeId { get; set; }

        public string DurationEnum { get; set; }
    }

    public class LiceneseExpireModel
    {
        public DateTime ExpiryDate { get; set; }
        public string DurationEnum { get; set; }
    }
}
