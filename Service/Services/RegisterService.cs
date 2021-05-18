using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Manages checkout by decreasing amount of bought products in db and returning a printed receipt
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<string> CheckOut(Cart cart)
        {
            var enrichedCart = await EnrichProductsInCart(cart);
            var receipt = _receiptService.CreateReceipt(enrichedCart);
            var text = _receiptService.PrintReceipt(receipt);
            
            foreach (var product in cart.Products)
            {
                await _productService.DecreaseProductAmount(product.Barcode, product.Amount);
            }
            return text;
        }

        private async Task<Cart> EnrichProductsInCart(Cart cart)
        {
            var enrichedCart = new Cart
            {
                Products = new List<Product>()
            };

            foreach (var product in cart.Products)
            {
                var productEnriched = await _productService.GetProduct(product.Barcode, false);
                productEnriched.Amount = product.Amount;
                enrichedCart.Products.Add(productEnriched);
            }            
            return enrichedCart;
        }
    }
}
