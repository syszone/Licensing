using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.Model;
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
    public class UpdateProductConfigurationQueryHandler : IRequestHandler<UpdateProductConfigurationQuery, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProductConfigurationQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> UpdateProductSKU(ProductWoocommViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("UpdateProductWCInfo",
                         new { ProductId = req.Id, WCProductId = req.WCProductId},
                          commandType: CommandType.StoredProcedure);

                    return 1;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public async Task<int> Handle(UpdateProductConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await UpdateProductSKU(request.Queryparamters);
        }
    }
}
