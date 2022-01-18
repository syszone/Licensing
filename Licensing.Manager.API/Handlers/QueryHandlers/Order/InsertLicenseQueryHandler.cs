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

namespace Licensing.Manager.API.Handlers.QueryHandlers.Order
{
    public class InsertLicenseQueryHandler : IRequestHandler<InsertLicenseQuery, int>
    {
        private readonly ProductLicensingDbContext _context;

        public InsertLicenseQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveLicense(LicenseQueryParameters req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertLicense",
                        new { ProductId = req.ProductId, LicenseId = req.LicenseId, Type = req.Type, Quantity = req.Quantity,
                              Name = req.Name,Email = req.Email,ExpiryDate = req.ExpiryDate,Signature = req.Signature,
                            LicenseUrl = req.licenseUrl
                        },
                          commandType: CommandType.StoredProcedure);

                   
                    return 1;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    

        public async Task<int> Handle(InsertLicenseQuery request, CancellationToken cancellationToken)
        {
            return await SaveLicense(request.Queryparamters);
        }
    }
}
