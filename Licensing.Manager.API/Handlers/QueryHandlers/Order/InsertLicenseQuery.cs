using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Order
{
    public class InsertLicenseQuery : IRequest<int>
    {
        public LicenseQueryParameters Queryparamters { get; set; }
        public InsertLicenseQuery(LicenseQueryParameters queryparamters)
        {
            Queryparamters = queryparamters;
        }
    }
}
