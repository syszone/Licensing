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
    public class UpdateProductConfigurationQueryHandler : IRequestHandler<UpdateProductConfigurationQuery, int>
    {
        private readonly ProductLicensingDbContext _context;

        public UpdateProductConfigurationQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }
        public async Task<int> UpdateProductSKU(ProductWooCommParameters req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("UpdateProductWCId",
                         new { ID = req.Id, WCProductId = req.WCProductId},
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
