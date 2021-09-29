using Licensing.Manager.ViewModels;
using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class ListProductFromDatabaseQuery :IRequest<List<ProductsVM>>
    {
    }
}
