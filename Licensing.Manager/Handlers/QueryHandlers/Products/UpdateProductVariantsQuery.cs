using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class UpdateProductVariantsQuery : IRequest<int>
    {
        public VarientWoocommViewModel Queryparamters { get; set; }
        public UpdateProductVariantsQuery(VarientWoocommViewModel queryparamters)
        {
            Queryparamters = queryparamters;
        }
    }
}
