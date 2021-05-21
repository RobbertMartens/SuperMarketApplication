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
        private readonly IMapperService _mapperService;

        public ReceiptService(IMapperService mapperService)
        {
            _mapperService = mapperService;
        }

        public Receipt CreateReceipt(Cart cart)
        {
            List<ReceiptProduct> receiptProducts = new List<ReceiptProduct>();

            foreach (var product in cart.Products)
            {
                receiptProducts.Add(_mapperService.MapReceiptProduct(product));
            }
            
            var receipt = _mapperService.MapReceipt(receiptProducts);
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

        private string PrintDiscount(ReceiptProduct product)
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
