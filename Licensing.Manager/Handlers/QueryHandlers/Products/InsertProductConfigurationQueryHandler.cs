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

namespace Licensing.Manager.Handlers.QueryHandlers.WooCommerce
{
    public class InsertProductConfigurationQueryHandler : IRequestHandler<InsertProductConfigurationQuery, int>
    {
        private readonly ApplicationDbContext _context;

        public InsertProductConfigurationQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveProductConfiguration(ProductConfigurationViewModel req)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("InsertProductConfiguration",
                        new { ProductId = req.ProductId, LicenseId = req.LicenseId, DurationId = req.DurationId, UserId = req.UserId, Name = req.Name, Description = req.Description, ShortDescription = req.ShortDescription, RegularPrice = req.RegularPrice, SalePrice = req.SalePrice, SKU = req.SKU, IsDownloadable = req.downloadable , Features = req.Features},
                          commandType: CommandType.StoredProcedure);

                    var productId = result.Select(r => new ProductIdViewModel { ProductId = r.ProductId }).ToList();
                    var Id = productId[0].ProductId;


                    for (int i = 0; i < req.Variants.Count; i++)
                    {
                        IEnumerable<dynamic> VariantResult = await connection.QueryAsync("InserVariations",
                                  new { ProductId = Id, LicenseDurationId = req.Variants[i].attributes[0].id,
                                      RegularPrice = Convert.ToDecimal(req.Variants[i].regular_price),
                                      SalesPrice = Convert.ToDecimal(req.Variants[i].sale_price),
                                      Description = req.Variants[i].description
                                  },
                                  commandType: CommandType.StoredProcedure);

                    }
                    for (int i = 0; i < req.downloadsfile.Count; i++)
                    {

                        IEnumerable<dynamic> downloadResult = await connection.QueryAsync("InsertProductDownload ",
                                  new { ProductId = Id, FileName = req.downloadsfile[i].name, FileURL = req.downloadsfile[i].file },
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
