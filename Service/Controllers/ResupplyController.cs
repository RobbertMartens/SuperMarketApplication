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

        public ResupplyController(ILogger<ResupplyController> logger, ILijpeVoorraadServerService voorraadService)
        {
            _voorraadService = voorraadService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> PostResupply(SupplyRequest Resupply)
        {
            if (Resupply == null || Resupply.ProvisionProducts == null)
            {
                return BadRequest();
            }
            try
            {
                await _voorraadService.ProcessResupplyAmounts(Resupply);
                return new OkObjectResult(Resupply);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
