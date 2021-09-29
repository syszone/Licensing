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
    public class ListLicenseTypeDurationQueryHandler : IRequestHandler<ListLicenseTypeDurationQuery, List<LicenseTypeDurationViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public ListLicenseTypeDurationQueryHandler(ApplicationDbContext context)
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
                        new { LicenseId = request.LicenseId },
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
