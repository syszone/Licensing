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
    public class InsertProductFeatureQueryHandler : IRequestHandler<InsertProductFeatureQuery, int>
    {

        public async Task<int> InsertProductFeature(ProductFeatureViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertUpdateProductFeature",
                        new { Id = req.Id,Name = req.Name, Value = req.Value },
                          commandType: CommandType.StoredProcedure);


                    return 1;
                }
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        public async Task<int> Handle(InsertProductFeatureQuery request, CancellationToken cancellationToken)
        {
            return await InsertProductFeature(request.QueryParameters);
        }
    }
}
