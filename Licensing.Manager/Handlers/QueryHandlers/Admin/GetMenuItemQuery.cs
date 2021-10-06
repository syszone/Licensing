using Licensing.Manager.Model;
using Licensing.Manager.Models;
using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Admin
{
    public class GetMenuItemQuery : IRequest<List<MenuItemModel>>
    {
        public RoleIdViewModel QueryParameters { get; set; }

        public GetMenuItemQuery(RoleIdViewModel queryParameters)
        {
            this.QueryParameters = queryParameters;

        }
    }
}
