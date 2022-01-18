using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string filename { get; set; }
        public string fileURL { get; set; }
        public int NumberOfDownloads { get; set; }

    }
}
