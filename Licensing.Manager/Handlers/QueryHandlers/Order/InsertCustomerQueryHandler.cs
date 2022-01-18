using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Handlers.QueryHandlers.WooCommerce;
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

namespace Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce
{
    public class InsertCustomerQueryHandler : IRequestHandler<InsertCustomerQuery, List<CustomerResponseViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public InsertCustomerQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerResponseViewModel>> SaveCustomerInfo(CustomerViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertCustomerInfo",
                       new { WCOrderId = req.WCOrderId, WCProductId = req.WCProductId, Name = req.Name, Company = req.Company, Email = req.Email, LicenseDurationId = req.LicenseDurationId, ProductLink = req.productLink },
                         commandType: CommandType.StoredProcedure);

                    var list = new List<CustomerResponseViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        list = result.Select(x => new CustomerResponseViewModel
                        {
                            LicenseType = x.LicenseType,
                            WCOrderId = x.WCOrderId
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
