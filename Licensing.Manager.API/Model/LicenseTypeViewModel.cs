using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class LicenseTypeViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }

    }

    public class ProductIdViewModel
    {
        public decimal ProductId { get; set; }
    }

}
