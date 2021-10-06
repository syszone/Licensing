using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.Model;
using Licensing.Manager.Models;
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
    public class GetMenuItemQueryHandler : IRequestHandler<GetMenuItemQuery, List<MenuItemModel>>
    {
        private readonly ApplicationDbContext _context;
        public GetMenuItemQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<MenuItemModel>> GetMenuItemFromDatabase(RoleIdViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetMenuItem",
                         new { RoleId = req.RoleId },
                        commandType: CommandType.StoredProcedure);

                    var MenuItemList = new List<MenuItemModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new MenuItemModel
                        {
                            Id = x.Id,
                            MenuName = x.MenuName,
                            ParentManuName = x.ParentMenuName,
                            HasAccess = x.HasAccess
                        }).ToList();
                        MenuItemList = await Task.FromResult(list);
                    }
                    return MenuItemList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<List<MenuItemModel>> Handle(GetMenuItemQuery request, CancellationToken cancellationToken)
        {
            return await GetMenuItemFromDatabase(request.QueryParameters);
        }


    }
}
