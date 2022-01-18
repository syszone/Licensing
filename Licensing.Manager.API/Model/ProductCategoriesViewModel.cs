using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Model
{
    public class ProductCategoriesViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class ProductFromCategoryViewModel
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }

    }
}
