using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.ViewModels
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WCProductId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int NumberOfDownloads { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }

        public DateTime OrderCreatedDate { get; set; }

    }
}
