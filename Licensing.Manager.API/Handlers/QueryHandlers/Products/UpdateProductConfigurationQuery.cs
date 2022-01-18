using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Products
{
    public class UpdateProductConfigurationQuery : IRequest<int>
    {
        public ProductWooCommParameters Queryparamters { get; set; }
        public UpdateProductConfigurationQuery(ProductWooCommParameters queryparamters)
        {
            Queryparamters = queryparamters;
        }
    }
}
