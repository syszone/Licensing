using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
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
