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

namespace Licensing.Manager.API.Handlers.QueryHandlers.Products
{
    public class ListProductLicenseQueryHandler : IRequestHandler<ListProductLicenseQuery, IEnumerable<ProductLicenseViewModel>>
    {
        private readonly ProductLicensingDbContext _context;
        public ListProductLicenseQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }
        private async Task<List<ProductLicenseViewModel>> GetProductLicenseFromDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetLicenseList",
                        commandType: CommandType.StoredProcedure);

                    var licenseList = new List<ProductLicenseViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductLicenseViewModel
                        {
                            Id = x.Id,
                            ProductId = x.WCProductId,
                            Name=x.Name,
                            LicenseType = x.Type,
                            LicenseFileUrl=x.LicenseFileUrl,
                            CustomerName = x.CustomerName,
                            Email = x.Email

                        }).ToList();
                        licenseList = await Task.FromResult(list);
                    }
                    return licenseList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;

        }
        public async Task<IEnumerable<ProductLicenseViewModel>> Handle(ListProductLicenseQuery request, CancellationToken cancellationToken)
        {
            return await GetProductLicenseFromDatabase();
        }
    }
}
