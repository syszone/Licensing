using Dapper;
using Licensing.Manager.Helper;
using Licensing.Manager.ViewModels.WoocommerceModels;
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
    public class DeleteProductVariantQueryHandler : IRequestHandler<DeleteProductVariantQuery, int>
    {
        private async Task<int> DeleteKeyword(ProductVarientViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("DeleteVariantIdWise",
                         new { VariantId = req.id },
                        commandType: CommandType.StoredProcedure);

                    return 1;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public async Task<int> Handle(DeleteProductVariantQuery request, CancellationToken cancellationToken)
        {
            if (request == null || request.QueryParameters == null)
                throw new ArgumentException(nameof(request));

            return await DeleteKeyword(request.QueryParameters);
        }
    }
}
