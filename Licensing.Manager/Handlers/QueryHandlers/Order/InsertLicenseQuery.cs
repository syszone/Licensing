using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Order
{
    public class InsertLicenseQuery : IRequest<int>
    {
        public LicenseViewModel Queryparamters { get; set; }
        public InsertLicenseQuery(LicenseViewModel queryparamters)
        {
            Queryparamters = queryparamters;
        }
    }
}
