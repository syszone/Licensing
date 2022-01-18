using Dapper;
using Licensing.Manager.API.Common;
using Licensing.Manager.API.Context;
using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class ListLicenseTypeDurationQueryHandler : IRequestHandler<ListLicenseTypeDurationQuery, List<LicenseTypeDurationViewModel>>
    {
        private readonly ProductLicensingDbContext _context;

        public ListLicenseTypeDurationQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }

        public async Task<List<LicenseTypeDurationViewModel>> Handle(ListLicenseTypeDurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetLicenseTypeduration",
                        new { LicenseId=request.LicenseId },
                        commandType: CommandType.StoredProcedure);

                    var LicenseTypeList = new List<LicenseTypeDurationViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new LicenseTypeDurationViewModel
                        {
                            Id = x.Id,
                            Duration = x.Duration,
                            LicenseId = x.LicenseId

                        }).ToList();
                        LicenseTypeList = await Task.FromResult(list);
                    }
                    return LicenseTypeList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
