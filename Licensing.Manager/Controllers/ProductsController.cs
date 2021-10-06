using Licensing.Manager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Licensing.Manager.ViewModels;
using Licensing;
using System.Xml.Linq;
using Licensing.Manager.Extension;
using Microsoft.AspNetCore.Authorization;
using Licensing.Manager.General;
using Licensing.Manager.ViewModels.WoocommerceModels;
using AutoMapper;
using Licensing.Manager.Model;
using MediatR;
using Licensing.Manager.Handlers.QueryHandlers.Products;
using Licensing.Manager.Handlers.QueryHandlers.WooCommerce;
using Licensing.Manager.Handlers;
using System.Net;
using System.IO.Compression;

namespace Licensing.Manager.Controllers
{
    [Authorize]
    //[CanAccessFilter]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        public IConfiguration configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        public ProductsController(IConfiguration config, IHostingEnvironment hostingEnvironment, IMapper mapper, IMediator mediator)
        {
            configuration = config;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _mediator = mediator;

        }

        [CanAccessFilter]
        public async Task<IActionResult> Create(int productId=0, int wcProductId=0)
        {
            ViewBag.ProductId = productId;
            ViewBag.WcProductId = wcProductId;

            return View();

        }

        [HttpPost]
        public async Task<JsonResult> CreateProduct(string Product, string License, string ProductVariation)
        {

            try
            {
                Woocommerce wc = new Woocommerce(configuration);
                ProductViewModel productObj = JsonConvert.DeserializeObject<ProductViewModel>(Product);
                LicenseDuration durationObj = JsonConvert.DeserializeObject<LicenseDuration>(License);
                List<ProductVarientViewModel> variationObj = JsonConvert.DeserializeObject<List<ProductVarientViewModel>>(ProductVariation);
                Features featureObj = JsonConvert.DeserializeObject<Features>(Product);

                productObj.date_created = DateTime.Now;

                if (Request.Form.Files.Count > 0)
                {
                    productObj.downloads = new List<Download>();
                    var basePath = configuration.GetSection("WoocommerceSettings").GetSection("WebURL").Value + "/uploads/";

                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var image = Request.Form.Files[i];
                        string filename = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
                        var fileExt = System.IO.Path.GetExtension(filename).Substring(1);
                        filename = System.DateTime.Now.ToString("ddMMyyyyhhmmsss") + "." + fileExt;
                        string FullPath = this._hostingEnvironment.WebRootPath + "\\uploads\\" + filename;

                        using (FileStream output = System.IO.File.Create(FullPath))
                            image.CopyTo(output);

                        productObj.downloads.Add(new Download { name = filename, file = (basePath + filename) });
                    }
                }
                var response = await SaveProductConfiguration(productObj.id, durationObj, productObj, variationObj, featureObj);

                
               

                return Json(new { IsSuccess = true,response});
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false });
            }

        }

        [HttpGet]
        public async Task<JsonResult> SyncToWoocommerce(int ProductId)
        {
            try
            {
                Woocommerce wc = new Woocommerce(configuration);
                var model = new ProductCategoryViewModel
                {
                    ProductId = ProductId
                };
                var response = await _mediator.Send(new GetProductForWoocommerceQuery(model));

                var productdata = response.product;
                if (productdata != null)
                {
                    ViewModels.WoocommerceModels.Product WCproduct = null;
                    if (productdata.id > 0)
                    {
                        productdata.date_modified = DateTime.Now;
                        var data = await wc.UpdateProduct(productdata.id, productdata);
                        if(data != null)
                        {
                            WCproduct = ((ViewModels.WoocommerceModels.Product)data);
                        }
                    }
                    else
                    {
                        productdata.date_created = DateTime.Now;
                        var data = await wc.CreateProduct(productdata);
                        if (data != null)
                        {
                            WCproduct = ((ViewModels.WoocommerceModels.Product)data);
                        }
                    }

                    if (WCproduct != null)
                    {
                        var newmodel = new ProductWoocommViewModel
                        {
                            Id = ProductId,
                            WCProductId = WCproduct.id.Value,
                        };

                        int result = await _mediator.Send(new UpdateProductConfigurationQuery(newmodel));

                        if (WCproduct.type != "simple")
                        {
                            if (WCproduct.variations != null && WCproduct.variations.Count > 0)
                            {
                                for (int i = 0; i < WCproduct.variations.Count; i++)
                                {
                                    var deleteVarient = await wc.DeleteVarient(WCproduct.id.Value, Convert.ToInt32(WCproduct.variations[i]));
                                }
                            }

                            var varientList = response.varients;
                            foreach (var item in varientList)
                            {
                                var varientId = item.id;
                                item.id = null;
                                var varient = await wc.CreateVarient(WCproduct.id.Value, item);
                                var WCVarient = ((ProductVariation)varient);
                                var varientModel = new VarientWoocommViewModel
                                {
                                    Id = varientId.Value,
                                    WCVarientId = WCVarient.id.Value,
                                };

                                int varientResult = await _mediator.Send(new UpdateProductVariantsQuery(varientModel));
                            }
                        }
                        return Json(new { IsSuccess = true });
                    }
                }
                return Json(new { IsSuccess = false });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false });
            }
        }

        [HttpGet]
        public async Task<string> GetCategoryByParent(string parent = "0")
        {
            try
            {
                Woocommerce wc = new Woocommerce(configuration);
                var data = await wc.GetCategoryByParent(parent);

                var pId = Convert.ToInt32(parent);
                IList<JSTree> category = data.Where(r => r.parent == pId).Select(r => new JSTree
                {
                    text = r.name,
                    parent = parent == "0" ? "#" : parent,
                    state = new State
                    {
                        opened = false,
                        disabled = false,
                        selected = false
                    },
                    li_attr = new JSTreeAttr
                    {
                        id = r.id.ToString(),
                        selected = false
                    }
                }).AsQueryable().ToList();

                var categoryJson = JsonConvert.SerializeObject(category.ToArray(), Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });

                return JsonConvert.SerializeObject(categoryJson, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpGet]
        public async Task<JsonResult> GetLicenseType(LicenseTypeViewModel model)
        {
            try
            {
                List<LicenseTypeViewModel> licensetypes = await _mediator.Send(new ListLicenseTypeQuery()); ;

                return Json(licensetypes);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpGet]
        public async Task<JsonResult> GetLicenseTypeDuration(int licenseId)
        {
            try
            {
                var model = new LicenseTypeDurationViewModel
                {
                    LicenseId = licenseId
                };
                List<LicenseTypeDurationViewModel> licensetypes = await _mediator.Send(new ListLicenseTypeDurationQuery(licenseId));

                return Json(licensetypes);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private async Task<int> SaveProductConfiguration(int ProductId, LicenseDuration durationObj, ProductViewModel productObj, List<ProductVarientViewModel> variantObj, Features featuresObj)
        {
            var durationId = durationObj.durationid == null ? "0" :  String.Join(",", durationObj.durationid);
            var featureId = String.Join(",", featuresObj.FeaturesId);
            var UserId = User.GetUserId();
            var model = new ProductConfigurationViewModel
            {
                ProductId = ProductId,
                LicenseId = durationObj.licensetype,
                Name = productObj.name,
                Description = productObj.description,
                ShortDescription = productObj.short_description,
                SKU = productObj.sku,
                RegularPrice = Decimal.Parse(productObj.regular_price),
                SalePrice = Decimal.Parse(productObj.sale_price),
                categories = productObj.categories,
                UserId = UserId,
                DurationId = durationId,
                Variants = variantObj,
                Features = featureId,
                downloadsfile = productObj.downloads,
                downloadable = productObj.downloadable,
                meta_data=productObj.meta_data

            };

            return await _mediator.Send(new InsertProductConfigurationQuery(model));
        }

        [CanAccessFilter]
        public async Task<IActionResult> Download()
        {

           
            return View();
        }

        public async Task<JsonResult> DownloadList(int PrdocuctId)
        {
            var model = new ProductLicenseViewModel {Id= PrdocuctId };
            var listOfLicense = await _mediator.Send(new ListProductLicenseQuery(model));
           
            return Json(listOfLicense);
        }

        [CanAccessFilter]
        public IActionResult SyncWithWoocommerce()
        {
            return View();
        }

        [CanAccessFilter]
        public async Task<IActionResult> DownloadLicense()
        {
            var listOfImage = await _mediator.Send(new ListProductImageQuery());
            return View(listOfImage);
        }

        [CanAccessFilter]
        public IActionResult Feature()
        {
            return View();
        }

        public async Task<JsonResult> FeatureList()
        {

            var FeatureList = await _mediator.Send(new GetAllProductFeatureQuery());

            return Json(FeatureList);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUpdateFeature(ProductFeatureViewModel model)
        {
            var response = await _mediator.Send(new InsertProductFeatureQuery(model));
            return Ok(response);
        }

        [HttpGet]
        public async Task<JsonResult> GetProductFeatures(int id)
        {
            var response = await _mediator.Send(new GetProductFeatureQuery(id));
            return Json(response);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteFeature(int featureId)
        {
            var model = new ProductFeatureViewModel { Id = featureId };
            var Delete = await _mediator.Send(new DeleteProductFeatureQuery(model));
            return Json(Delete);

        }

        [HttpGet]
        public async Task<JsonResult> GetProductData(int ProductId, int wcProductId)
        {
            Woocommerce wc = new Woocommerce(configuration);
            var model = new ProductCategoryViewModel
            {
                ProductId = ProductId,
                WcProductId = wcProductId.ToString()


            };
            var response = await _mediator.Send(new GetProductForWoocommerceQuery(model));
            return Json(response);
        }


        [HttpGet]
        public async Task<string> DownloadFiles(int productId)
        {
            try
            {
                var folder = DateTime.Now.Ticks.ToString();
                var ServerPath = _hostingEnvironment.WebRootPath + "\\Downloads\\";
                if (!Directory.Exists(ServerPath))
                {
                    Directory.CreateDirectory(ServerPath);
                }
                ServerPath = ServerPath + "\\" + folder + "\\";
                if (!Directory.Exists(ServerPath))
                {
                    Directory.CreateDirectory(ServerPath);
                }
                var model = new ProductLicenseViewModel { Id = productId };
                var listOfLicense = await _mediator.Send(new ListProductLicenseQuery(model));

                foreach (var item in listOfLicense)
                {
                    var url = item.FileUrl;
                    Uri uriResult;

                    bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                    if (result)
                    {
                        using (var client = new WebClient())
                        {
                            var content = client.DownloadData(url);
                            System.IO.File.WriteAllBytes($"{ServerPath}/{Path.GetFileName(url)}", content);
                        }
                    }
                }
                return folder;
            }
            catch (Exception ex)
            {

                return "";
            }
            
        }

        public ActionResult DownloadZip(string folder)
        {
            var ServerPath = _hostingEnvironment.WebRootPath + "\\Downloads\\" + folder;
            DirectoryInfo dirInfo = new DirectoryInfo(ServerPath);
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    foreach (var item in dirInfo.GetFiles())
                    {
                        var path = item.FullName;
                        var filename = item.Name;
                        ziparchive.CreateEntryFromFile(path, filename);
                    }
                return File(memoryStream.ToArray(), "application/zip", DateTime.Now.ToShortDateString().Replace("/", "") + "-Files.zip");
            }
        }

    }
}
