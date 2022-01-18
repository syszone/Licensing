using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class ProductConfiguration
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int LicenseId { get; set; }
        public string DurationId { get; set; }
        public string UserId { get; set; }

    }
}
