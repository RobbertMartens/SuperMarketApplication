using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplyController : ControllerBase
    {
        private readonly ILogger<SupplyController> _logger;
        private readonly ISupplyService _supplyService;
        private readonly IProductService _productService;

        public SupplyController(ILogger<SupplyController> logger, ISupplyService voorraadService, IProductService productService)
        {
            _supplyService = voorraadService;
            _productService = productService;
            _logger = logger;
        }

        [HttpPut]
        public async Task<IActionResult> PutSupply(List<Supply> supplies)
        {
            if (supplies == null || supplies.Count == 0)
            {
                return new BadRequestResult();
            }
            try
            {
                await _supplyService.ProcessResupplyAmounts(supplies);
                return new OkObjectResult(supplies);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentSupplies()
        {
            try
            {
                var supplies = await _supplyService.GetCurrentSupplies();
                return new OkObjectResult(supplies);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> PostNewProduct(Product product)
        {
            if (product == null) { return new BadRequestResult(); }

            try
            {
                await _productService.InsertProduct(product);
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}
