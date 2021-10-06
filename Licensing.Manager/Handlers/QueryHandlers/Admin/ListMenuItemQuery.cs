using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Admin
{
    public class ListMenuItemQuery : IRequest<List<MenuItemViewModel>>
    {
        public SearchMenuItemViewModel QueryParameters { get; set; }

        public ListMenuItemQuery(SearchMenuItemViewModel queryParameters)
        {
            this.QueryParameters = queryParameters;

        }
    }
}
