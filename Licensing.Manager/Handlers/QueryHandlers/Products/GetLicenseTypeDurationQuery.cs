using Licensing.Manager.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Handlers.QueryHandlers.Products
{
    public class GetLicenseTypeDurationQuery :  IRequest<List<LicenseTypeDurationViewModel>>
    {
        public string Type { get; set; }
        public string Duration { get; set; }

        public GetLicenseTypeDurationQuery(string type,string duration)
        {
            Type = type;
            Duration = duration;
        }
    }
}
