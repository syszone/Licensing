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
    public class InsertProductConfigurationQueryHandler : IRequestHandler<InsertProductConfigurationQuery, int>
    {
        private readonly ProductLicensingDbContext _context;

        public InsertProductConfigurationQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveProductConfiguration(ProductConfigurationParameters req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertProductConfiguration",
                        new { ProductId = req.ProductId, LicenseId = req.LicenseId, DurationId = req.DurationId, UserId = req.UserId, Name = req.Name, Description = req.Description, ShortDescription = req.ShortDescription, RegularPrice = req.RegularPrice, SalePrice = req.SalePrice, SKU = req.SKU, IsDownloadable = req.downloadable },
                          commandType: CommandType.StoredProcedure);

                    var productId = result.Select(r => new ProductIdViewModel { ProductId = r.ProductId }).ToList();
                    var Id = productId[0].ProductId;

                    for (int i = 0; i < req.downloadsfile.Count; i++)
                    {

                        IEnumerable<dynamic> imageResult = await connection.QueryAsync("InserProductImage",
                            new { ProductId = Id, FileName = req.downloadsfile[i].name, FileUrl = req.downloadsfile[i].file },
                            commandType: CommandType.StoredProcedure);
                    }

                    for (int i = 0; i < req.categories.Count; i++)
                    {
                        IEnumerable<dynamic> categoryResult = await connection.QueryAsync("InsertCategories",
                               new { ProductId = Id, CategoryId = req.categories[i].id, CategoryName = req.categories[i].name },
                               commandType: CommandType.StoredProcedure);
                    }

                    return 1;
                }

            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public async Task<int> Handle(InsertProductConfigurationQuery request, CancellationToken cancellationToken)
        {
            return await SaveProductConfiguration(request.Queryparamters);
        }
    }
}
