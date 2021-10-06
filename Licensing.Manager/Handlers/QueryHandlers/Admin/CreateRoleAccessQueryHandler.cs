using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Admin
{
    public class CreateRoleAccessQueryHandler : IRequestHandler<CreateRoleAccessQuery, int>
    {
        private readonly ApplicationDbContext _context;
        public CreateRoleAccessQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> createRoleAccess(CreateRoleAccessQuery req)
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder("", 50);
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                string query = "";
                queryBuilder.Append("DELETE FROM MenuAccess WHERE RoleId = '" + req.QueryParameters.RoleId + "'  ");

                foreach (var item in req.QueryParameters.RolesItem)
                {
                    queryBuilder.Append("\n INSERT INTO MenuAccess (RoleId, MenuId, HasAccess) VALUES('" + req.QueryParameters.RoleId + "','" + item.Id + "','" + item.Selected + "')  ");
                }

                queryBuilder.Append("  ");
                query = queryBuilder.ToString();
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("ExecuteQuery",
                        new { SQLQuery = query },
                        commandType: CommandType.StoredProcedure);

                    return 1;

                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public Task<int> Handle(CreateRoleAccessQuery request, CancellationToken cancellationToken)
        {
            return createRoleAccess(request);
        }
    }
}
