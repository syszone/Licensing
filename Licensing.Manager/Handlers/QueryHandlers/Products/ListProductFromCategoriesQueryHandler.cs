using Dapper;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.ViewModels;
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
    public class ListProductFromCategoriesQueryHandler : IRequestHandler<ListProductFromCategoriesQuery, List<ProductFromCategoryViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public ListProductFromCategoriesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public async Task<List<ProductFromCategoryViewModel>> Handle(ListProductFromCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductfromCategories",
                        new { CategoryId = request.CategoryId },
                        commandType: CommandType.StoredProcedure);

                    var ProductList = new List<ProductFromCategoryViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductFromCategoryViewModel
                        {
                            ProductId = x.Id,
                            Name = x.Name,
                            CategoryId = x.CategoryId

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
    }
}
