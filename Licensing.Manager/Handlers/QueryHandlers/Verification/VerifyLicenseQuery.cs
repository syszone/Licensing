using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Verification
{
    public class VerifyLicenseQuery : IRequest<VerifyResponseModel>
    {
        public MachineKeysViewModel QueryParameters { get; set; }

        public VerifyLicenseQuery(MachineKeysViewModel queryparameters)
        {
            QueryParameters = queryparameters;
        }
    }
}
