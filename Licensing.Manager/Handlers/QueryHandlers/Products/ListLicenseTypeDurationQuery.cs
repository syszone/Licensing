using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class ListLicenseTypeDurationQuery : IRequest<List<LicenseTypeDurationViewModel>>
    {
        public int LicenseId { get; private set; }

        public ListLicenseTypeDurationQuery(int licenseId)
        {
            this.LicenseId = licenseId;
        }
    }
}
