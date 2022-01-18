using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class LicenseTypeDurationViewModel
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public string Duration { get; set; }
    }
}
