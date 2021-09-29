using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class GetProductForWoocommerceQuery : IRequest<ProductResponse>
    {
        public ProductCategoryViewModel QueryParameters { get; private set; }

        public GetProductForWoocommerceQuery(ProductCategoryViewModel queryParameter)
        {
            this.QueryParameters = queryParameter;
        }
    }
}
