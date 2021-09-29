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

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class ListLicenseTypeQueryHandler : IRequestHandler<ListLicenseTypeQuery, List<LicenseTypeViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public ListLicenseTypeQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<LicenseTypeViewModel>> GetLicenseType()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetLicenseType",
                        commandType: CommandType.StoredProcedure);

                    var LicenseList = new List<LicenseTypeViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new LicenseTypeViewModel
                        {
                            Id = x.Id,
                            Type = x.Type

                        }).ToList();
                        LicenseList = await Task.FromResult(list);
                    }
                    return LicenseList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<LicenseTypeViewModel>> Handle(ListLicenseTypeQuery request, CancellationToken cancellationToken)
        {
            return await GetLicenseType();
        }
    }
}
