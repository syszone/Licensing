using Licensing.Manager.Model;
using Licensing.Manager.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Admin
{
    public class CreateRoleAccessQuery : IRequest<int>
    {
        public RoleAccessModel QueryParameters { get; set; }
        public CreateRoleAccessQuery(RoleAccessModel queryParameters)
        {
            this.QueryParameters = queryParameters;
        }
    }
}
