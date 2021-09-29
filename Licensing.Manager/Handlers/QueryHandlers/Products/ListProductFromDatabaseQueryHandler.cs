using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.ViewModels;
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
    public class ListProductFromDatabaseQueryHandler : IRequestHandler<ListProductFromDatabaseQuery, List<ProductsVM>>
    {
        private readonly ApplicationDbContext _context;

        public ListProductFromDatabaseQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<ProductsVM>> GetProducts()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductFromDatabase",
                        commandType: CommandType.StoredProcedure);

                    var ProductList = new List<ProductsVM>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductsVM
                        {
                            id = x.Id,
                            name = x.Name,
                            description = x.Description,
                            short_description = x.ShortDescription,
                            regular_price = Convert.ToString(x.RegularPrice),
                            sale_price = Convert.ToString(x.SalePrice),
                            durations = x.Durations,
                            licensetype = x.LicenseType,
                            wcProductId = x.WcProductId == null ? "0" : x.WcProductId.ToString()

                        }).ToList();
                        ProductList = await Task.FromResult(list);
                    }
                    return ProductList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<ProductsVM>> Handle(ListProductFromDatabaseQuery request, CancellationToken cancellationToken)
        {
            return await GetProducts();
        }
    }
}
