﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Licensing.Manager.API.Model;
using MediatR;

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class ListLicenseTypeQuery : IRequest<IEnumerable<LicenseTypeViewModel>>
    {

    }
}
