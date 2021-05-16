using Service.Interfaces;
using Service.Models;
using System;
using System.Threading.Tasks;

namespace Service.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;

        public RegisterService(IReceiptService calculatePriceService, IProductService productService)
        {
            _receiptService = calculatePriceService;
            _productService = productService;
        }

        public async Task<string> CheckOut(Cart cart)
        {
            var receipt = _receiptService.CreateReceipt(cart);
            var text = _receiptService.PrintReceipt(receipt);
            
            foreach (var product in receipt.BoughtProducts)
            {
                await _productService.DecreaseProductAmount(product.Barcode, product.Amount);
            }
            Console.WriteLine(receipt);
            return text;
        }
    }
}
