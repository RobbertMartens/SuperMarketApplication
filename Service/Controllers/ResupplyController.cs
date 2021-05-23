using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResupplyController : ControllerBase
    {
        private readonly ILogger<ResupplyController> _logger;
        private readonly ILijpeVoorraadServerService _voorraadService;
        private readonly IProductService _productService;

        public ResupplyController(ILogger<ResupplyController> logger, ILijpeVoorraadServerService voorraadService, IProductService productService)
        {
            _voorraadService = voorraadService;
            _productService = productService;
            _logger = logger;
        }

        [HttpPut]
        public async Task<IActionResult> PutResupply(SupplyRequest Resupply)
        {
            if (Resupply == null || Resupply.ProductsToSupply == null)
            {
                return new BadRequestResult();
            }
            try
            {
                await _voorraadService.ProcessResupplyAmounts(Resupply);
                return new OkObjectResult(Resupply);
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
                var supplies = await _voorraadService.GetCurrentSupplies();
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
