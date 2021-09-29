using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers
{
    public class DeleteProductFeatureQuery : IRequest<int>
    {
        public ProductFeatureViewModel QueryParameters { get; set; }

        public DeleteProductFeatureQuery(ProductFeatureViewModel queryParameters)
        {
            this.QueryParameters = queryParameters;

        }
    }
}
