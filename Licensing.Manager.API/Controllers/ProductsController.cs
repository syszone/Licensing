using AutoMapper;
using Licensing.Manager.API.Handlers.QueryHandlers.Products;
using Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce;
using Licensing.Manager.API.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licensing.Manager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<ProductsController> _logger;
        public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("GetLicenseType")]
        public async Task<IEnumerable<LicenseTypeViewModel>> GetLicenseType()
        {
            return await _mediator.Send(new ListLicenseTypeQuery());
        }

        [HttpGet("GetProductConfiguration")]
        public async Task<IEnumerable<ProductConfigurationViewModel>> GetProductConfiguration()
        {
            return await _mediator.Send(new ListProductConfigurationQuery());
        }

        [HttpPost("SaveProductConfiguration")]
        public async Task<ActionResult<int>> SaveProductConfiguration(ProductConfigurationViewModel req)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductConfigurationViewModel, ProductConfigurationParameters>();
            });

            IMapper iMapper = config.CreateMapper();

            ProductConfigurationParameters destination = iMapper.Map<ProductConfigurationViewModel, ProductConfigurationParameters>(req);

            var result = await _mediator.Send(new InsertProductConfigurationQuery(destination));
            return 1;
        }

        //GetLicenseTypeduration
        [HttpPost("GetLicenseTypeduration")]
        public async Task<ActionResult<LicenseTypeDurationViewModel>> GetLicenseTypeduration(LicenseTypeDurationViewModel req )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LicenseTypeDurationViewModel, LicenseTypeDurationParameter>();
            });

            IMapper iMapper = config.CreateMapper();
            LicenseTypeDurationParameter destination = iMapper.Map<LicenseTypeDurationViewModel, LicenseTypeDurationParameter>(req);

            var result = await _mediator.Send(new ListLicenseTypeDurationQuery(req.LicenseId));
            return Ok(result);
        }

        [HttpGet("GetProductImageFromDatabase")]
        public async Task<IEnumerable<ProductImageViewModel>> GetProductImageFromDatabase()
        {
            return await _mediator.Send(new ListProductImageQuery());
        }
        [HttpGet("GetCategories")]
        public async Task<IEnumerable<ProductCategoriesViewModel>> GetCategories()
        {
            return await _mediator.Send(new ListCategoriesQuery());
        }
         


        [HttpPost("ProductFromCategory")]
        public async Task<ActionResult<ProductFromCategoryViewModel>> ProductFromCategory(ProductFromCategoryViewModel req)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductFromCategoryViewModel, ProductCategoryParameters>();
            });

            IMapper iMapper = config.CreateMapper();
            ProductCategoryParameters destination = iMapper.Map<ProductFromCategoryViewModel, ProductCategoryParameters>(req);

            var result = await _mediator.Send(new ListProductFromCategoriesQuery(req.CategoryId));
            return Ok(result);

        }

        [HttpPost("ProductFromDataBase")]
        public async Task<ActionResult<ProductViewModel>> ProductFromDataBase(ProductViewModel req)
        {
            

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductViewModel, ProductQueryParameter>();
            });

            IMapper iMapper = config.CreateMapper();
            ProductQueryParameter destination = iMapper.Map<ProductViewModel, ProductQueryParameter>(req);

            var result = await _mediator.Send(new GetProductForWoocommerceQuery(destination));
            return Ok(result);

        }

        [HttpPost("UpdateProductSKU")]
        public async Task<ActionResult<int>> UpdateProductSKU(ProductWoocommViewModel req)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductWoocommViewModel, ProductWooCommParameters>();
            });

            IMapper iMapper = config.CreateMapper();

            ProductWooCommParameters destination = iMapper.Map<ProductWoocommViewModel, ProductWooCommParameters>(req);

            var result = await _mediator.Send(new UpdateProductConfigurationQuery(destination));
            return 1;
        }
        [HttpGet("GetProductLicenseFromDatabase")]
        public async Task<IEnumerable<ProductLicenseViewModel>> GetProductLicenseFromDatabase()
        {
            return await _mediator.Send(new ListProductLicenseQuery());
        }
    }
}
