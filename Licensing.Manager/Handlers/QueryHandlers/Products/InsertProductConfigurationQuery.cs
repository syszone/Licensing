using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.WooCommerce
{
    public class InsertProductConfigurationQuery : IRequest<int>
    {
        public ProductConfigurationViewModel Queryparamters { get; set; }
        public InsertProductConfigurationQuery(ProductConfigurationViewModel queryparamters)
        {
            Queryparamters = queryparamters;
        }

    }
}
