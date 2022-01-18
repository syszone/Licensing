using AutoMapper;
using Licensing.Manager.API.Handlers.QueryHandlers.Order;
using Licensing.Manager.API.Handlers.QueryHandlers.WooCommerce;
using Licensing.Manager.API.Model;
using MediatR;
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
    public class OrderController : ControllerBase
    {

        private readonly IMediator _mediator;

        private readonly ILogger<OrderController> _logger;
        public OrderController(ILogger<OrderController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("SaveCustomerInfo")]
        public async Task<ActionResult<List<CustomerResponseViewModel>>> SaveCustomerInfo(CustomerViewModel req)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerViewModel, CustomerQueryParameters>();
            });

            IMapper iMapper = config.CreateMapper();

            CustomerQueryParameters destination = iMapper.Map<CustomerViewModel, CustomerQueryParameters>(req);

            var result = await _mediator.Send(new InsertCustomerQuery(destination));
            return result;
        }

        [HttpPost("SaveLicense")]
        public async Task<ActionResult<int>> SaveLicense(LicenseViewModel req)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LicenseViewModel, LicenseQueryParameters>();
            });

            IMapper iMapper = config.CreateMapper();

            LicenseQueryParameters destination = iMapper.Map<LicenseViewModel, LicenseQueryParameters>(req);

            var result = await _mediator.Send(new InsertLicenseQuery(destination));
            return 1;
        }
    }
}
