using AutoMapper;
using Licensing.Manager.Model;
using Licensing.Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.Helper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProductConfigurationViewModel, ProductConfigurationParameters>();            
        }
    }
}
