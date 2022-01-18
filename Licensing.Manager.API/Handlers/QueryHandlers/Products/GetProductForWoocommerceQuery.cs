using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Products
{
    public class GetProductForWoocommerceQuery : IRequest<ProductViewModel>
    {
        public ProductQueryParameter QueryParameters { get; private set; }

        public GetProductForWoocommerceQuery(ProductQueryParameter queryParameter)
        {
            this.QueryParameters = queryParameter;
        }
    }
}
