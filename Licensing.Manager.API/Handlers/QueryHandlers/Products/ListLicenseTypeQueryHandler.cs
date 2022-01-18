using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Licensing.Manager.API.Common;
using Licensing.Manager.API.Context;
using Licensing.Manager.API.Model;
using MediatR;

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class ListLicenseTypeQueryHandler : IRequestHandler<ListLicenseTypeQuery, IEnumerable<LicenseTypeViewModel>>
    {
        private readonly ProductLicensingDbContext _context;

        public ListLicenseTypeQueryHandler(ProductLicensingDbContext context)
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

        public async Task<IEnumerable<LicenseTypeViewModel>> Handle(ListLicenseTypeQuery request, CancellationToken cancellationToken)
        {
            return await GetLicenseType();
        }
    }
}
