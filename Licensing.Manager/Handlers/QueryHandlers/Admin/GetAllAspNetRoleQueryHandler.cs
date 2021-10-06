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
    public class GetAllAspNetRoleQueryHandler :IRequestHandler<GetAllAspNetRoleQuery, List<AspNetRoleViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllAspNetRoleQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<AspNetRoleViewModel>> GetAllAspNetRoleList()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetAspNetRole",
                        commandType: CommandType.StoredProcedure);

                    var RoleList = new List<AspNetRoleViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new AspNetRoleViewModel
                        {

                            RoleId = x.RoleId,
                            Name = x.Name

                        }).ToList();
                        RoleList = await Task.FromResult(list);
                    }
                    return RoleList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<AspNetRoleViewModel>> Handle(GetAllAspNetRoleQuery request, CancellationToken cancellationToken)
        {
            return await GetAllAspNetRoleList();
        }
    }
}
