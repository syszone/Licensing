using Dapper;
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
    public class GetLicenseTypeDurationQueryHandler : IRequestHandler<GetLicenseTypeDurationQuery, List<LicenseTypeDurationViewModel>>
    {
        public async Task<List<LicenseTypeDurationViewModel>> GetLicenseTypeList(string Type, string Duration)
        {
            try
            {
                List<LicenseTypeDurationViewModel> LicenseTypeList = new List<LicenseTypeDurationViewModel>();

                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("ListLicenseTypeDuration",
                          new { Type = Type , Duration = Duration },
                          commandType: CommandType.StoredProcedure);

                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new LicenseTypeDurationViewModel
                        {
                            Id = x.Id,
                            Duration = x.Duration,
                            LicenseId = x.LicenseId,
                            DurationEnum = x.DurationEnum

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

        public async Task<List<LicenseTypeDurationViewModel>> Handle(GetLicenseTypeDurationQuery request, CancellationToken cancellationToken)
        {
            return await  GetLicenseTypeList(request.Type,request.Duration);
        }
    }
}
