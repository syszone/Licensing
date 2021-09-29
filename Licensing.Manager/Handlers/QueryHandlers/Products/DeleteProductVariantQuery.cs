using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class DeleteProductVariantQuery : IRequest<int>
    {
        public ProductVarientViewModel QueryParameters { get; set; }

        public DeleteProductVariantQuery(ProductVarientViewModel queryParameters)
        {
            this.QueryParameters = queryParameters;

        }
    }
}
