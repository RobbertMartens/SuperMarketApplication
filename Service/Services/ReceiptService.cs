using Service.Enum;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;

namespace Service.Services
{
    public class ReceiptService : IReceiptService
    {
        private const string _format = "{0, -25}{1, -10}{2, 15}{3, 15}{4, 15}";
        private readonly ICalculateProductPrice _calculateProductPrice;

        public ReceiptService(ICalculateProductPrice calculateProductPrice)
        {
            _calculateProductPrice = calculateProductPrice;
        }

        public Receipt CreateReceipt(Cart cart)
        {
            var receipt = new Receipt
            {
                Message = "Bedankt dat u bij de Boni bent geweest!",
                TimePrinted = DateTime.Now,
                BoughtProducts = new List<ProductReceipt>()
            };
            
            foreach (var product in cart.Products)
            {
                var productReceipt = new ProductReceipt();
                productReceipt.ProductName = product.ProductName;
                productReceipt.Barcode = product.Barcode;
                productReceipt.Amount = product.Amount;
                productReceipt.Discount = product.Discount;
                productReceipt.ProductPrice = product.Price;
                productReceipt.ProductPriceWithDiscount = _calculateProductPrice.Calculate(product, 1);
                productReceipt.Total = _calculateProductPrice.Calculate(product, product.Amount);

                receipt.BoughtProducts.Add(productReceipt);
                receipt.TotalPrice += productReceipt.Total;
            }
            return receipt;
        }

        public string PrintReceipt(Receipt receipt)
        {
            var printedReceipt = $"{receipt.Message}\n";
            printedReceipt += DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "\n";
            printedReceipt += string.Format(_format, "Naam", "Aantal", "Prijs", "Korting", "Subtotaal") + "\n";

            foreach (var product in receipt.BoughtProducts)
            {
                printedReceipt += string.Format(_format, product.ProductName, product.Amount, product.ProductPrice, 
                    PrintDiscount(product), product.Total) + "\n";
            }
            printedReceipt += $"Totaal: {receipt.TotalPrice}";

            return printedReceipt;
        }

        private string PrintDiscount(ProductReceipt product)
        {
            if (product.Discount == Discount.NoDiscount)
            {
                return "";
            }
            else
            {
                return $"-{(product.Amount * product.ProductPrice) - (product.Amount * product.ProductPriceWithDiscount)}  ";
            }
        }
    }
}
