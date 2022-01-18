using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Products
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
