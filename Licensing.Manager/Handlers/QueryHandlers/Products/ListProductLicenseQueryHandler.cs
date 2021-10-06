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
    public class ListProductLicenseQueryHandler : IRequestHandler<ListProductLicenseQuery, IEnumerable<ProductLicenseViewModel>>
    {
        private readonly ApplicationDbContext _context;
        public ListProductLicenseQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<List<ProductLicenseViewModel>> GetProductLicenseFromDatabase(ProductLicenseViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductDownloads", new { ProductId =req.Id },
                        commandType: CommandType.StoredProcedure);

                    var licenseList = new List<ProductLicenseViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductLicenseViewModel
                        {
                            Id=x.Id,
                            WcProductId = x.WCProductId,
                            LicenseURL=x.LicenseURL,
                            CustomerName = x.CustomerName,
                            Email = x.Email,
                            OrderCreatedDate=x.OrderCreated,
                            WCOrderId=x.WCOrderId,
                            FileName=x.FileName,
                            FileUrl=x.FileUrl

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
            return await GetProductLicenseFromDatabase(request.QueryParameters);
        }
    }
}
