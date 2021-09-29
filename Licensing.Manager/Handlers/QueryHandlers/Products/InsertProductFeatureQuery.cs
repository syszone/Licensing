using System;
using System.Collections.Generic;
using System.Linq;
using Licensing.Manager.ViewModels;
using System.Threading.Tasks;
using MediatR;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class InsertProductFeatureQuery : IRequest<int>
    {
        public ProductFeatureViewModel QueryParameters { get; set; }

        public InsertProductFeatureQuery(ProductFeatureViewModel queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }
}
