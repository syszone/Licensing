using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.ViewModels;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Admin
{
    public class ListMenuItemQueryHandler : IRequestHandler<ListMenuItemQuery, List<MenuItemViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public ListMenuItemQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<List<MenuItemViewModel>> GetDataQueueFromDatabase(SearchMenuItemViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetUserMenuAccess",
                         new { UserId = req.UserId },
                        commandType: CommandType.StoredProcedure);

                    var menuList = new List<MenuItemViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new MenuItemViewModel
                        {
                            Id = x.Id,
                            MenuName = x.MenuName,
                            ParentManuName = x.ParentManuName,
                            HasAccess = x.HasAccess,
                            UserId = x.UserId,
                            URL = x.URL
                        }).ToList();
                        menuList = await Task.FromResult(list);
                    }
                    return menuList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<List<MenuItemViewModel>> Handle(ListMenuItemQuery request, CancellationToken cancellationToken)
        {
            return await GetDataQueueFromDatabase(request.QueryParameters);
        }
    }
}
