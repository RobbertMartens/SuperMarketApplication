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
    public class RegisterController : ControllerBase
    {
        ILogger<RegisterController> _logger;
        IRegisterService _registerService;

        public RegisterController(ILogger<RegisterController> logger, IRegisterService registerService)
        {
            _logger = logger;
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<ActionResult> PostCheckOut(Cart cart)
        {
            if (cart == null || cart.Products == null) { return BadRequest(); }

            var printedReceipt = _registerService.CheckOut(cart);

            return new OkObjectResult(printedReceipt);
        }
    }
}
