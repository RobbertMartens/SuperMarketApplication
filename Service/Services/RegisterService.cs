using Service.Interfaces;
using Service.Models;
using System;

namespace Service.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly ICalculateCartPrice _calculateCartPriceService;

        public RegisterService(ICalculateCartPrice calculatePriceService)
        {
            _calculateCartPriceService = calculatePriceService;
        }

        public string CheckOut(Cart cart)
        {
            var totalPrice = _calculateCartPriceService.Calculate(cart);
            var receipt = $"Totaalbedrag: {totalPrice}";
            Console.WriteLine(receipt);
            return receipt;
        }
    }
}
