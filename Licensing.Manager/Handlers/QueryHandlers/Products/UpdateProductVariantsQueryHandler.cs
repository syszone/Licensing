using Dapper;
using Licensing.Manager.Data;
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
    public class UpdateProductVariantsQueryHandler : IRequestHandler<UpdateProductVariantsQuery, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProductVariantsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> UpdateProductSKU(VarientWoocommViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("UpdateVarientWCInfo",
                         new { VarientId = req.Id, WCVarientId = req.WCVarientId },
                          commandType: CommandType.StoredProcedure);

                    return 1;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public async Task<int> Handle(UpdateProductVariantsQuery request, CancellationToken cancellationToken)
        {
            return await UpdateProductSKU(request.Queryparamters);
        }
    }
}
