using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class ListProductFromCategoriesQuery : IRequest<List<ProductFromCategoryViewModel>>
    {

        public int CategoryId { get; private set; }

        public ListProductFromCategoriesQuery(int categoryId)
        {
            this.CategoryId = categoryId;
        }
    }
}
