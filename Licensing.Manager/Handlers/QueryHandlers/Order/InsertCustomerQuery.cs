using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.WooCommerce
{
    public class InsertCustomerQuery : IRequest<List<CustomerResponseViewModel>>
    {
        public CustomerViewModel Queryparamters { get; set; }
        public InsertCustomerQuery(CustomerViewModel queryparamters)
        {
            Queryparamters = queryparamters;
        }

    }
}
