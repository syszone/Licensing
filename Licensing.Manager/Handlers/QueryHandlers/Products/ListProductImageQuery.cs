using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.WooCommerce
{
    public class ListProductImageQuery: IRequest<IEnumerable<ProductImageViewModel>>
    {
    }
}
