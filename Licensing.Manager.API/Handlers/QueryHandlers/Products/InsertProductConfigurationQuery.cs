using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class InsertProductConfigurationQuery : IRequest<int>
    {
        public ProductConfigurationParameters Queryparamters { get; set; }
        public InsertProductConfigurationQuery(ProductConfigurationParameters queryparamters)
        {
            Queryparamters = queryparamters;
        }

    }
}
