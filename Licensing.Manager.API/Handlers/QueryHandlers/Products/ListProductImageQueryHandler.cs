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
    public class ListProductImageQueryHandler : IRequestHandler<ListProductImageQuery, IEnumerable<ProductImageViewModel>>
    {
        private readonly ProductLicensingDbContext _context;
        public ListProductImageQueryHandler(ProductLicensingDbContext context)
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
                            ProductId=x.WCProductId,
                            FileName = x.FileName,
                            FileUrl = x.FileUrl,
                            CustomerName = x.CustomerName,
                            Email = x.Email

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
