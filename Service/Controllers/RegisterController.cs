using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using Service.Models;
using System;
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
        public async Task<IActionResult> PostCheckOut(Cart cart)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("model is valid");
            }
            if (cart == null || cart.Products == null) { return BadRequest(); }

            try
            {
                var printedReceipt = await _registerService.CheckOut(cart);
                return new OkObjectResult(printedReceipt);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }            
        }
    }
}
