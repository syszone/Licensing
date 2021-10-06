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
    public class GetAllAsoNetUsersQueryHandler : IRequestHandler<GetAllAsoNetUsersQuery, List<AspNetUserViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllAsoNetUsersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<AspNetUserViewModel>> GetAllAspNetUserList()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetAspNetUsers",
                        commandType: CommandType.StoredProcedure);

                    var UserList = new List<AspNetUserViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new AspNetUserViewModel
                        {
                            UserId = x.UserId,
                            UserName = x.UserName,
                            Email = x.Email,
                            FullName = x.FullName,
                            Phone = x.Phone,
                            Address1 = x.Address1,
                            Address2 = x.Address2,
                            City = x.City,
                            Country = x.Country,
                            RoleId = x.RoleId,
                            RoleName = x.RoleName,
                            IsActive = x.IsActive


                        }).ToList();
                        UserList = await Task.FromResult(list);
                    }
                    return UserList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<AspNetUserViewModel>> Handle(GetAllAsoNetUsersQuery request, CancellationToken cancellationToken)
        {
            return await GetAllAspNetUserList();
        }
    }
}
