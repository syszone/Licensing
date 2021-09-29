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
    public class ListProductFeatureQueryHandler : IRequestHandler<ListProductFeatureQuery, List<ProductFeatureViewModel>>
    {

        public async Task<List<ProductFeatureViewModel>> GetProductFeature(int id)
        {
            try
            {
                List<ProductFeatureViewModel> ProductFeatureList = new List<ProductFeatureViewModel>();

                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductFeature",
                        new { WCProductId = id },
                          commandType: CommandType.StoredProcedure);

                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductFeatureViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Value = x.Value

                        }).ToList();
                        ProductFeatureList = await Task.FromResult(list);
                    }
                    return ProductFeatureList;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<List<ProductFeatureViewModel>> Handle(ListProductFeatureQuery request, CancellationToken cancellationToken)
        {
            return await GetProductFeature(request.Id);
        }
    }
}
