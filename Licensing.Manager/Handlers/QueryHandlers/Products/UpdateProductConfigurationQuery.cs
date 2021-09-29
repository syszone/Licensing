using Licensing.Manager.Model;
using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class UpdateProductConfigurationQuery : IRequest<int>
    {
        public ProductWoocommViewModel Queryparamters { get; set; }
        public UpdateProductConfigurationQuery(ProductWoocommViewModel queryparamters)
        {
            Queryparamters = queryparamters;
        }
    }
}
