using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class MapperService : IMapperService
    {
        private readonly ICalculateProductPrice _calculateProductPrice;

        public MapperService(ICalculateProductPrice calculateProductPrice)
        {
            _calculateProductPrice = calculateProductPrice;
        }

        public Receipt MapReceipt(IEnumerable<ReceiptProduct> receiptProducts)
        {
            if (receiptProducts == null) { throw new NullReferenceException("Received receipt products are null!"); }

            var receipt = new Receipt
            {
                Message = "Bedankt dat u bij de Boni bent geweest!",
                TimePrinted = DateTime.Now,
                BoughtProducts = new List<ReceiptProduct>()
            };

            try
            {
                foreach (var receiptProduct in receiptProducts)
                {
                    receipt.BoughtProducts.Add(receiptProduct);
                    receipt.TotalPrice += receiptProduct.Total;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                throw;
            }
            return receipt;
        }

        public ReceiptProduct MapReceiptProduct(Product product)
        {
            if (product == null) { throw new NullReferenceException($"Receveived product is null!"); }

            var productReceipt = new ReceiptProduct();

            try
            {
                productReceipt.ProductName = product.ProductName;
                productReceipt.Barcode = product.Barcode;
                productReceipt.Amount = product.Amount;
                productReceipt.Discount = product.Discount;
                productReceipt.ProductPrice = product.Price;
                productReceipt.ProductPriceWithDiscount = _calculateProductPrice.Calculate(product, 1);
                productReceipt.Total = _calculateProductPrice.Calculate(product, product.Amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.InnerException);
                throw;
            }
            return productReceipt;
        }

        public IEnumerable<Supply> MapSupplyRequest(IEnumerable<Product> products)
        {
            if (products == null) { throw new NullReferenceException("Received products is null"); }

            var supplies = new List<Supply>();

            foreach (var product in products)
            {
                supplies.Add(new Supply
                {
                    Amount = product.Amount,
                    Barcode = product.Barcode
                });
            }
            return supplies;
        }
    }
}
