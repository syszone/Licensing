using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Verification
{
    public class VerifyLicenseQuery : IRequest<bool>
    {
        public MachineKeysViewModel QueryParameters { get; set; }

        public VerifyLicenseQuery(MachineKeysViewModel queryparameters)
        {
            QueryParameters = queryparameters;
        }
    }
}
