using Dapper;
using Licensing.Manager.API.General;
using Licensing.Manager.Data;
using Licensing.Manager.Helper;
using Licensing.Manager.Model;
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
    public class GetProductForWoocommerceQueryHandler : IRequestHandler<GetProductForWoocommerceQuery, ProductResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetProductForWoocommerceQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ProductResponse> ProductFromDataBase(ProductCategoryViewModel res)
        {
            ProductResponse response = new ProductResponse();
            try
            {
                using (var connection = new SqlConnection(Utils.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    IEnumerable<dynamic> result = await connection.QueryAsync("GetProductFromDatabase",
                        new { ProductId = res.ProductId },
                        commandType: CommandType.StoredProcedure);

                    ProductViewModel Products = new ProductViewModel();
                    List<ProductVarientViewModel> varientList = new List<ProductVarientViewModel>();

                    if (result != null && result.Count() > 0)
                    {
                        Products = result.Select(x => new ProductViewModel
                        {
                            name = x.Name,
                            description = x.Description,
                            short_description = x.ShortDescription,
                            regular_price = Convert.ToString(x.RegularPrice),
                            sale_price = Convert.ToString(x.SalePrice),
                            sku = DateTime.Now.Ticks.ToString(),
                            downloadable = x.IsDownloadable,

                        }).FirstOrDefault();
                        var wcProductId = result.Select(x => x.WcProductId).FirstOrDefault();
                        if (wcProductId > 0)
                        {
                            Products.id = wcProductId;
                        }

                        response.Features = result.Select(x => x.Features).FirstOrDefault();
                        response.licenseType = result.Select(x => x.LicenseId).FirstOrDefault();
                        response.Durations = result.Select(x => x.DurationId).FirstOrDefault();

                        var LicenseId = result.Select(x => x.LicenseId).FirstOrDefault();

                        IEnumerable<dynamic> VarientImages = await connection.QueryAsync("GetProductDownloads",
                                   new { ProductId = res.ProductId },
                                   commandType: CommandType.StoredProcedure);

                        if (LicenseId != (int)LicenseType.Standard)
                        {
                            var WCAttributeId = result.Select(x => x.WCAttributeId).FirstOrDefault();
                            var DurationId = result.Select(x => x.DurationId).FirstOrDefault();
                            var DurationList = DurationId.Split(",");
                            List<string> options = new List<string>();
                            Products.attributes = new List<Variation_Attributes>();

                            foreach (var item in DurationList)
                            {
                                var id = Convert.ToInt32(item);
                                string description = ((LicenseTypeDuration)id).GetDescription();
                                options.Add(description);
                            }

                            Variation_Attributes attribute = new Variation_Attributes
                            {
                                id = WCAttributeId.ToString(),
                                name = ((LicenseType)LicenseId).ToString(),
                                options = options
                            };
                            Products.attributes.Add(attribute);

                            Default_Variation_Attributes default_attributes = new Default_Variation_Attributes
                            {
                                name = ((LicenseType)LicenseId).ToString(),
                                options = options[0]
                            };
                            Products.default_attributes = new List<Default_Variation_Attributes>();
                            Products.default_attributes.Add(default_attributes);

                            IEnumerable<GetProductVarientResult> varients = await connection.QueryAsync<GetProductVarientResult>("GetProductVarient",
                                new { ProductId = res.ProductId },
                                commandType: CommandType.StoredProcedure);

                            if (varients != null && varients.Count() > 0)
                            {

                                foreach (var item in varients)
                                {
                                    var varient = new ProductVarientViewModel
                                    {
                                        id = item.Id,
                                        sku = DateTime.Now.Ticks.ToString(),
                                        price = item.RegularPrice.ToString(),
                                        regular_price = item.RegularPrice.ToString(),
                                        sale_price = item.SalesPrice.ToString(),
                                        on_sale = item.SalesPrice > 0 ? true : false,
                                        description = item.Description,
                                        date_created = DateTime.Now,
                                        attributes = new List<ProductAttributes>()
                                        {
                                            new ProductAttributes
                                            {
                                                id = item.WCAttributeId.ToString(),
                                                name = item.Type,
                                                option = item.Duration
                                            }
                                        }
                                    };

                                    if (VarientImages != null && VarientImages.Count() > 0)
                                    {
                                        var Imagelist = VarientImages.Select(x => new Download
                                        {
                                            name = x.FileName,
                                            file = x.FileUrl
                                        }).ToList();
                                        varient.downloads = new List<Download>();
                                        varient.downloads = Imagelist;
                                        varient.downloadable = true;
                                    }
                                    varientList.Add(varient);
                                }

                            }

                            response.varients = varientList;

                        }
                        else
                        {
                            Products.type = "simple";
                            if (VarientImages != null && VarientImages.Count() > 0)
                            {
                                var Imagelist = VarientImages.Select(x => new Download
                                {
                                    name = x.FileName,
                                    file = x.FileUrl
                                }).ToList();
                                Products.downloads = new List<Download>();
                                Products.downloads = Imagelist;
                                Products.downloadable = true;
                            }

                        }

                        //Get Tab Here
                        Products.meta_data = new List<Meta_Data>();
                        IEnumerable<GetProductTabResult> TabResult = await connection.QueryAsync<GetProductTabResult>("GetProductTab",
                           new { ProductId = res.ProductId },
                           commandType: CommandType.StoredProcedure);
                        if (TabResult != null && TabResult.Count() > 0)
                        {
                            var tabs = new Meta_Data
                            {
                                key = "yikes_woo_products_tabs",
                                value = new List<Meta_Data_Value>()
                            };
                            foreach (var item in TabResult)
                            {
                                var tab = new Meta_Data_Value
                                {

                                    title = item.Name,
                                    id = item.Name.Replace(" ", "-"),
                                    content = item.Contents
                                };
                                tabs.value.Add(tab);
                            }
                            Products.meta_data.Add(tabs);
                        }

                        IEnumerable<GetProductCategoryResult> categories = await connection.QueryAsync<GetProductCategoryResult>("GetCategories",
                           new { ProductId = res.ProductId },
                           commandType: CommandType.StoredProcedure);

                        if (categories != null && categories.Count() > 0)
                        {
                            var categoryList = categories.Select(x => new Category
                            {
                                id = x.CategoryId,
                                name = x.CategoryName
                            }).ToList();
                            Products.categories = new List<Category>();
                            Products.categories = categoryList;
                        }

                        response.product = Products;
                        response = await Task.FromResult(response);

                        return response;

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<ProductResponse> Handle(GetProductForWoocommerceQuery request, CancellationToken cancellationToken)
        {
            if (request == null || request.QueryParameters == null)
                throw new ArgumentException(nameof(request));

            return await ProductFromDataBase(request.QueryParameters);

        }
    }
}
