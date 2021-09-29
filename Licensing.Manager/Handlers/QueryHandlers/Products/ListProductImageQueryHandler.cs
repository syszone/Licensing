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

namespace Licensing.Manager.Handlers.QueryHandlers.WooCommerce
{
    public class ListProductImageQueryHandler : IRequestHandler<ListProductImageQuery, IEnumerable<ProductImageViewModel>>
    {
        private readonly ApplicationDbContext _context;
        public ListProductImageQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<List<ProductImageViewModel>> GetProductImageFromDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    IEnumerable<dynamic> result = await connection.QueryAsync("GetImageList",                         
                        commandType: CommandType.StoredProcedure);

                    var ImageList = new List<ProductImageViewModel>();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductImageViewModel
                        {
                            Id=x.Id,
                            WCProductId=x.WCProductId,
                            FileName = x.FileName,
                            FileUrl = x.FileUrl,
                            CustomerName = x.CustomerName,
                            Email = x.Email,
                            OrderCreatedDate=x.OrderCreated

                        }).ToList();
                        ImageList = await Task.FromResult(list);
                    }
                    return ImageList;
                }
            }
            catch (Exception ex)
            {

            }
            return null;

        }
        public async Task<IEnumerable<ProductImageViewModel>> Handle(ListProductImageQuery request, CancellationToken cancellationToken)
        {
            return await GetProductImageFromDatabase();
        }
    }
}
