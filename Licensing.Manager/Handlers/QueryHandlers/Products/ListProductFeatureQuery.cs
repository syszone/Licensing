using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class ListProductFeatureQuery : IRequest<List<ProductFeatureViewModel>>
    {
        public int Id { get; set; }

        public ListProductFeatureQuery(int _Id)
        {
            Id = _Id;
        }
    }
}
