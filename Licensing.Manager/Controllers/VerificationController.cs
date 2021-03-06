using Microsoft.Extensions.Configuration;
using Licensing.Manager.Handlers.QueryHandlers.Verification;
using Licensing.Manager.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Licensing.Validation;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace Licensing.Manager.Controllers
{
    public class VerificationController : Controller
    {
        private readonly IMediator _mediator;
        public IConfiguration _configuration;

        public VerificationController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<VerifyResponseModel> VerifyLicense(MachineKeysViewModel request)
        {
            VerifyResponseModel response = new VerifyResponseModel();
            try
            {
                MachineKeysViewModel KeyObject = new MachineKeysViewModel();
                KeyObject = request;

                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    var file = Request.Form.Files[i];
                    string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileExt = System.IO.Path.GetExtension(filename).Substring(1);
                    if (fileExt.ToLower() == "lic")
                    {
                        var result = new StringBuilder();
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                                result.AppendLine(await reader.ReadLineAsync());
                        }

                        XmlSerializer serializer = new XmlSerializer(typeof(License));
                        using (StringReader reader = new StringReader(result.ToString()))
                        {
                            KeyObject.license = License.Load(result.ToString());
                        }
                        var data = KeyObject;
                        response = await _mediator.Send(new VerifyLicenseQuery(KeyObject));
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Invalid FileType";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        

    }
}
