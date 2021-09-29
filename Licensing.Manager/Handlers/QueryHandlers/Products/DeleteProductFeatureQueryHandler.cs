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
    public class DeleteProductFeatureQueryHandler : IRequestHandler<DeleteProductFeatureQuery, int>
    {
        
        private async Task<int> DeleteKeyword(ProductFeatureViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("DeleteProductFeaturesIdWise",
                         new { FeatureId = req.Id },
                        commandType: CommandType.StoredProcedure);

                    return 1;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public async Task<int> Handle(DeleteProductFeatureQuery request, CancellationToken cancellationToken)
        {
            if (request == null || request.QueryParameters == null)
                throw new ArgumentException(nameof(request));

            return await DeleteKeyword(request.QueryParameters);
        }
    }
}
