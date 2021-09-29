using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Order
{
    public class InsertLicenseQueryHandler : IRequestHandler<InsertLicenseQuery, int>
    {
        private readonly ApplicationDbContext _context;

        public InsertLicenseQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveLicense(LicenseViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertLicense",
                        new {
                            WCOrderId = req.WCOrderId, LicenseId = req.LicenseId, Type = req.Type, Quantity = req.Quantity,
                              Name = req.Name,Email = req.Email,ExpiryDate = req.ExpiryDate,Signature = req.Signature,
                            LicenseUrl = req.licenseUrl,
                            PublicKey = req.PublicKey
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
