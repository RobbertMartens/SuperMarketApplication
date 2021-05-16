using Service.Interfaces;
using Service.Models;
using System;

namespace Service.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IReceiptService _receiptService;

        public RegisterService(IReceiptService calculatePriceService)
        {
            _receiptService = calculatePriceService;
        }

        public string CheckOut(Cart cart)
        {
            var receipt = _receiptService.CreateReceipt(cart);
            var text = _receiptService.PrintReceipt(receipt);
            // Decrease Product amounts
            Console.WriteLine(receipt);
            return text;
        }
    }
}
