using Licensing.Manager.API.Handlers.QueryHandlers.Verification;
using Licensing.Manager.API.Model;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Licensing.Manager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMediator _mediator;
        public VerificationController(IHostingEnvironment hostingEnvironment, IMediator mediator)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> VerifyLicense([FromForm] MachineKeysViewModel request)
        {
            try
            {
                MachineKeysViewModel KeyObject = new MachineKeysViewModel();
                KeyObject = request;

                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    var file = Request.Form.Files[i];                    
                    var result = new StringBuilder();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            result.AppendLine(await reader.ReadLineAsync());
                    }
                    
                    Model.License obj = new Model.License();
                    XmlSerializer serializer = new XmlSerializer(typeof(Licensing.Manager.API.Model.License));
                    using (StringReader reader = new StringReader(result.ToString()))
                    {
                        Licensing.Manager.API.Model.License license = (Licensing.Manager.API.Model.License)(serializer.Deserialize(reader));                        
                        KeyObject.license = license;
                    }
                    var data = KeyObject;
                    var response = await _mediator.Send(new VerifyLicenseQuery(KeyObject));
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
              
            }
            return Ok(null);
        }
    }
}
