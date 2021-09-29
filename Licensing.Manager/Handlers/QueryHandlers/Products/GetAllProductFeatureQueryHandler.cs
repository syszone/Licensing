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
    public class GetAllProductFeatureQueryHandler : IRequestHandler<GetAllProductFeatureQuery, List<ProductFeatureViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllProductFeatureQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<ProductFeatureViewModel>> GetAllFeatureList()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetAllProductFeatures",
                        commandType: CommandType.StoredProcedure);

                    var FeatureList = new List<ProductFeatureViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductFeatureViewModel
                        {
                           Id = x.Id,
                           Name = x.Name,
                          Value = x.Value,
                            

                        }).ToList();
                        FeatureList = await Task.FromResult(list);
                    }
                    return FeatureList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<ProductFeatureViewModel>> Handle(GetAllProductFeatureQuery request, CancellationToken cancellationToken)
        {
            return await GetAllFeatureList();
        }
    }
}
