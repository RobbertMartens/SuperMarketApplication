using Moq;
using NUnit.Framework;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System;
using System.Collections.Generic;

namespace Service.UnitTests.Services
{
    public class ReceiptServiceUnitTests
    {
        private Mock<ICalculateProductPrice> _calculateProductPriceMock;
        private Mock<IMapperService> _mapperServiceMock;
        private Cart _cart;

        [SetUp]
        public void Setup()
        {
            _calculateProductPriceMock = new Mock<ICalculateProductPrice>();
            _mapperServiceMock = new Mock<IMapperService>();

            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M, Amount = 1 });
        }

        [Test]
        public void CreateReceipt_ShouldReturnCorrectDouble_WhenGivenCartNoDiscount()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0], 1)).Returns(_cart.Products[0].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[1], 1)).Returns(_cart.Products[1].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[2], 1)).Returns(_cart.Products[2].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[3], 1)).Returns(_cart.Products[3].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[4], 1)).Returns(_cart.Products[4].Price);

            _mapperServiceMock.Setup(mock => mock.MapReceiptProduct(_cart.Products[0])).
                Returns(new ReceiptProduct {
                    Amount = 1,
                    Barcode = 123,
                    Discount = Discount.NoDiscount,
                    ProductName = "Kaas",
                    ProductPrice = 4.99M,
                    Total = 4.99M });

            _mapperServiceMock.Setup(mock => mock.MapReceipt(It.IsAny<List<ReceiptProduct>>())).Returns(new Receipt
            {
                Message = "hooooiii test",
                TimePrinted = DateTime.Now,
                TotalPrice = 4.99M
            });

            var receiptService = new ReceiptService(_mapperServiceMock.Object);

            // Assign
            var expectedPrice = 4.99;

            // Act
            var receipt = receiptService.CreateReceipt(_cart);

            // Assert
            Assert.AreEqual(expectedPrice, receipt.TotalPrice);
        }

        [Test]
        public void PrintReceipt_WithFiveProduct_ShouldReturnPrintedReceipt()
        {
            // Asseble
            var receiptService = new ReceiptService(_mapperServiceMock.Object);
            var receipt = new Receipt
            {
                Message = "Bedankt Hoooiiii",
                TimePrinted = DateTime.Now,
                TotalPrice = 5.59M,
                BoughtProducts = new List<ReceiptProduct>
                {
                    new ReceiptProduct
                    {
                        Amount = 1,
                        Barcode = 123,
                        Discount = Discount.Bonus,
                        ProductName = "Kaas",
                        ProductPrice = 4.99M,
                        ProductPriceWithDiscount = 4.00M,
                        Total = 4.99m
                    }
                }
            };

            // Act
            var printedReceipt = receiptService.PrintReceipt(receipt);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(printedReceipt.Contains("Naam"));
                Assert.That(printedReceipt.Contains("Aantal"));
                Assert.That(printedReceipt.Contains("Prijs"));
                Assert.That(printedReceipt.Contains("Korting"));
                Assert.That(printedReceipt.Contains("Subtotaal"));
                Assert.That(printedReceipt.Contains("Totaal"));
                Assert.That(printedReceipt.Contains("Bedankt"));
                Assert.That(printedReceipt.Contains(","));
                Assert.That(printedReceipt.Contains("1"));
            });
        }
    }
}
