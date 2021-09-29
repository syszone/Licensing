using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class GetProductFeatureQuery : IRequest<List<ProductFeatureViewModel>>
    {
        public int Id { get; set; }

        public GetProductFeatureQuery(int _Id)
        {
            Id = _Id;
        }
    }
}
