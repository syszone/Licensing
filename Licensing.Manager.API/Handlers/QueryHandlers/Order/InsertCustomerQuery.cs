using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class InsertCustomerQuery : IRequest<List<CustomerResponseViewModel>>
    {
        public CustomerQueryParameters Queryparamters { get; set; }
        public InsertCustomerQuery(CustomerQueryParameters queryparamters)
        {
            Queryparamters = queryparamters;
        }

    }
}
