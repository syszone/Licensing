using Dapper;
using Licensing.Manager.API.Common;
using Licensing.Manager.API.Context;
using Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce;
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
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, IEnumerable<ProductCategoriesViewModel>>
    {
        private readonly ProductLicensingDbContext _context;

        public ListCategoriesQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }
        private async Task<List<ProductCategoriesViewModel>> GetCategories()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetCategories",
                        commandType: CommandType.StoredProcedure);

                    var CategoryList = new List<ProductCategoriesViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductCategoriesViewModel
                        {
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryName

                        }).ToList();
                        CategoryList = await Task.FromResult(list);
                    }
                    return CategoryList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<IEnumerable<ProductCategoriesViewModel>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await GetCategories();
        }
    }
}
