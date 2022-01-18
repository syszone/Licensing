using Licensing.Manager.General;
using Licensing.Manager.Handlers.QueryHandlers.Order;
using Licensing.Manager.Handlers.QueryHandlers.Products;
using Licensing.Manager.Handlers.QueryHandlers.WooCommerce;
using Licensing.Manager.Interface;
using Licensing.Manager.Models;
using Licensing.Manager.ViewModels;
using Licensing.Manager.ViewModels.WoocommerceModels;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Licensing.Manager.Controllers
{
    public class OrderController : Controller
    {
        private readonly IEmailSend _emailSender;
        public IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMediator _mediator;
        public OrderController(IConfiguration configuration, IEmailSend emailSender, IHostingEnvironment hostingEnvironment, IMediator mediator)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<int> OrderCreate([FromBody] OrderViewModel request)
        {
            try
            {
                if (request.line_items.Count > 0 && request.billing != null)
                {
                    List<Line_Items> ListItems = request.line_items;
                    List<LicenseDurationViewModel> LicenseDuration = new List<LicenseDurationViewModel>();
                    foreach (var item in ListItems)
                    {
                        foreach (var itemMeta in item.meta_data)
                        {
                            LicenseDuration.Add(itemMeta);
                        }
                    }
                    List<LicenseTypeDurationViewModel> duration = new List<LicenseTypeDurationViewModel>();
                    if (LicenseDuration.Count > 0)
                    {
                        duration = await _mediator.Send(new GetLicenseTypeDurationQuery(LicenseDuration[0].display_key, LicenseDuration[0].display_value));
                    }

                    var productId = request.line_items.FirstOrDefault().product_id;
                    Woocommerce wc = new Woocommerce(_configuration);
                    var product = await wc.GetProducts(productId);

                    CustomerViewModel _customer = new CustomerViewModel
                    {
                        WCOrderId = request.id,
                        WCProductId = productId,
                        Name = request.billing.first_name + " " + request.billing.last_name,
                        Company = request.billing.company,
                        Email = request.billing.email,
                        LicenseDurationId = duration.Count > 0 ? duration[0].Id : 0,
                        productLink = product.permalink
                    };
                    var data = await _mediator.Send(new InsertCustomerQuery(_customer));
                    if (data != null && !string.IsNullOrEmpty(_customer.Email))
                    {
                        var result = data.FirstOrDefault();

                        List<ProductFeatureViewModel> ProdcutFeaturesList = await _mediator.Send(new ListProductFeatureQuery(_customer.WCProductId));

                        var licenseResponse = generateLicence(_customer.Name, _customer.Email, result.LicenseType, ProdcutFeaturesList, request.line_items[0].quantity);

                        var fileURL = _configuration.GetSection("WoocommerceSettings").GetSection("WebURL").Value;

                        var basePath = _hostingEnvironment.WebRootPath;
                        string keyPath = Path.Combine(basePath, "Key");
                        if (!Directory.Exists(keyPath))
                        {
                            Directory.CreateDirectory(keyPath);
                        }

                        var fileName = DateTime.Now.Ticks + ".lic";
                        keyPath = Path.Combine(keyPath, fileName);

                        fileURL = fileURL + "/Key/" + fileName;

                        var license = licenseResponse.License;
                        using (StreamWriter outputFile = new StreamWriter(keyPath))
                        {
                            outputFile.WriteLine(license.ToString());
                            outputFile.Close();
                        }

                        var model = new LicenseViewModel
                        {
                            LicenseId = license.Id.ToString(),
                            WCOrderId = result.WCOrderId,
                            licenseUrl = fileURL,
                            Type = license.Type.ToString(),
                            Quantity = license.Quantity.ToString(),
                            Name = license.Customer.Name.ToString(),
                            Email = license.Customer.Email.ToString(),
                            ExpiryDate = Convert.ToDateTime(license.Expiration.ToString()),
                            Signature = license.Signature.ToString(),
                            PublicKey = licenseResponse.PublicKey
                        };

                        var res = await _mediator.Send(new InsertLicenseQuery(model));
                        var Link = _configuration.GetSection("Link").GetSection("link").Value;
                        //Send Keypath to email<a href='" + response.Source + "' target='_blank'>Go to here <i class='fas fa-sign-out-alt'></i></a>
                        var link = "You can register by using this <a href='"+ Link + request.id + "'>link</a>";
                        await _emailSender.SendEmailAsync(_customer.Email, "License Key", $"Please find your licence key in the attachment." + link,
                         keyPath);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        public LicenseResponseViewModel generateLicence(string customerName, string customerEmail, int licenseType, List<ProductFeatureViewModel> productFeatureList, int quantity)
        {
            LicenseResponseViewModel response = new LicenseResponseViewModel();

            var passPhrase = _configuration.GetSection("LicenseSettings").GetSection("passPhrase").Value;
            var keyGenerator = Security.Cryptography.KeyGenerator.Create();
            var keyPair = keyGenerator.GenerateKeyPair();
            var privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            response.PublicKey = keyPair.ToPublicKeyString();

            var licenseId = Guid.NewGuid();

            Dictionary<string, string> productFeatures = new Dictionary<string, string>();
            foreach (var item in productFeatureList)
            {
                productFeatures.Add(item.Name, item.Value);
            }

            response.License = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As((LicenseType)licenseType)
                                 .WithMaximumUtilization(quantity)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            return response;
        }

        public async Task<JsonResult> EditProduct(int productId)
        {
            Woocommerce wc = new Woocommerce(_configuration);
            var data = await wc.GetProducts(productId);
            ProductDownloadLine _download = new ProductDownloadLine
            {
                name = "637678322599268737.txt",
                file = _configuration.GetSection("WoocommerceSettings").GetSection("WebURL").Value + "/Key/637678322599268737.txt"
            };
            data.downloads.Add(_download);
            var result = await wc.UpdateProduct(productId, data);
            return Json(result);
        }

    }
}
