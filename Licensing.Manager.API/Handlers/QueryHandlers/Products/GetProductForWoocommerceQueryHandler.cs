using Dapper;
using Licensing.Manager.API.Common;
using Licensing.Manager.API.Context;
using Licensing.Manager.API.General;
using Licensing.Manager.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Handlers.QueryHandlers.Products
{
    public class GetProductForWoocommerceQueryHandler : IRequestHandler<GetProductForWoocommerceQuery, ProductViewModel>
    {
        private readonly ProductLicensingDbContext _context;

        public GetProductForWoocommerceQueryHandler(ProductLicensingDbContext context)
        {
            _context = context;
        }
        

        public async Task<ProductViewModel> ProductFromDataBase(ProductQueryParameter res)
        {
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductFromDatabase",
                        new { ProductId = res.id },
                        commandType: CommandType.StoredProcedure);

                    ProductViewModel Products = new ProductViewModel();
                    if (result != null && result.Count() > 0)
                    {
                        var list = result.Select(x => new ProductViewModel
                        {
                           
                            name = x.Name,
                            description = x.Description,
                            short_description = x.ShortDescription,
                            regular_price = Convert.ToString(x.RegularPrice),
                            sale_price = Convert.ToString(x.SalePrice),
                            sku = x.SKU,
                            downloadable=x.IsDownloadable,

                    }).FirstOrDefault();
                        Products = await Task.FromResult(list);

                        IEnumerable<dynamic> imageResult = await connection.QueryAsync("GetProductImageList",
                        new { ProductId = res.id },
                        commandType: CommandType.StoredProcedure);

                        if (imageResult != null && imageResult.Count() > 0)
                        {
                            var Imagelist = imageResult.Select(x => new Download
                            {
                                name = x.FileName,
                                file = x.FileUrl


                            }).ToList();
                            Products.downloads = new List<Download>();
                            Products.downloads = Imagelist;
                        }


                        var LicenseId = result.Select(x => x.LicenseId).FirstOrDefault();

                        var DurationId = result.Select(x => x.DurationId).FirstOrDefault();
                        var DurationList = DurationId.Split(",");
                        List<string> variation = new List<string>();
                        Products.variations = new List<string>();

                        foreach (var item in DurationList)
                        {
                            var id = Convert.ToInt32(item);
                            string description = ((LicenseTypeDuration)id).GetDescription();
                            variation.Add(description);
                        }
 
                        Products.variations= variation;





                        //Products = await Task.FromResult(list);
                    }



                    return Products;

                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<ProductViewModel> Handle(GetProductForWoocommerceQuery request, CancellationToken cancellationToken)
        {
            if (request == null || request.QueryParameters == null)
                throw new ArgumentException(nameof(request));

            return await ProductFromDataBase(request.QueryParameters);

        }
    }
}
