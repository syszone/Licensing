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
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, List<ProductCategoriesViewModel>>
    {
        private readonly ApplicationDbContext _context;

        public ListCategoriesQueryHandler(ApplicationDbContext context)
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
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetCategories", new { ProductId =0},
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

        public async Task<List<ProductCategoriesViewModel>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await GetCategories();
        }
    }
}
