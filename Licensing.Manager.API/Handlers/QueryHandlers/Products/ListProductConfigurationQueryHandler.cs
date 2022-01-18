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
    public class ListProductConfigurationQueryHandler : IRequestHandler<ListProductConfigurationQuery, IEnumerable<ProductConfigurationViewModel>>
    {
        private readonly ProductLicensingDbContext _context;
        public ListProductConfigurationQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }

        private async Task<List<ProductConfigurationViewModel>> GetProductConfiguration()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductConfiguration",
                        commandType: CommandType.StoredProcedure);

                    var ProductConfigurationList = new List<ProductConfigurationViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductConfigurationViewModel
                        {
                            Id = x.Id,
                            LicenseId = x.LicenseId,
                            DurationId = x.DurationId,
                            UserId = x.UserId,
                            ProductId = x.ProductId

                        }).ToList();
                        ProductConfigurationList = await Task.FromResult(list);
                    }
                    return ProductConfigurationList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<IEnumerable<ProductConfigurationViewModel>> Handle(ListProductConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await GetProductConfiguration();
        }
    }
}
