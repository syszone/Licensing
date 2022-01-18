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

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class InsertCustomerQueryHandler : IRequestHandler<InsertCustomerQuery, List<CustomerResponseViewModel>>
    {
        private readonly ProductLicensingDbContext _context;

        public InsertCustomerQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerResponseViewModel>> SaveCustomerInfo(CustomerQueryParameters req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertCustomerInfo",
                        new { ProductId = req.ProductId, Name = req.Name, Company = req.Company, Email = req.Email },
                          commandType: CommandType.StoredProcedure);

                    var list = new List<CustomerResponseViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        list = result.Select(x => new CustomerResponseViewModel
                        {
                            LicenseType = x.LicenseType
                        }).ToList();
                    }
                    return list;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<CustomerResponseViewModel>> Handle(InsertCustomerQuery request, CancellationToken cancellationToken)
        {
            return await SaveCustomerInfo(request.Queryparamters);
        }
    }
}
