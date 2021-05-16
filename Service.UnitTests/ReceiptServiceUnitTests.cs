using Moq;
using NUnit.Framework;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using Service.Services;

namespace Service.UnitTests
{
    public class ReceiptServiceUnitTests
    {
        private Mock<ICalculateProductPrice> _calculateProductPriceMock;
        private Cart _cart;

        [SetUp]
        public void Setup()
        {
            _calculateProductPriceMock = new Mock<ICalculateProductPrice>();

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

            var receiptService = new ReceiptService(_calculateProductPriceMock.Object);

            // Assign
            var expectedPrice = 13.18M;

            // Act
            var receipt = receiptService.CreateReceipt(_cart);

            // Assert
            Assert.AreEqual(expectedPrice, receipt.TotalPrice);
        }

        [Test]
        public void CreateReceipt_WithFiveProducts_ShouldReturnReceiptObject()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0], 1)).Returns(_cart.Products[0].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[1], 2)).Returns(_cart.Products[1].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[2], 3)).Returns(_cart.Products[2].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[3], 4)).Returns(_cart.Products[3].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[4], 5)).Returns(_cart.Products[4].Price);
            var receiptService = new ReceiptService(_calculateProductPriceMock.Object);

            // Act
            var receipt = receiptService.CreateReceipt(_cart);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.Greater(receipt.TotalPrice, 1.00M);
                Assert.AreEqual(_cart.Products[0].ProductName, receipt.BoughtProducts[0].ProductName);
                Assert.AreEqual(_cart.Products[0].Barcode, receipt.BoughtProducts[0].Barcode);
                Assert.AreEqual(_cart.Products[0].Price, receipt.BoughtProducts[0].ProductPrice);
                Assert.AreEqual(Discount.NoDiscount, receipt.BoughtProducts[0].Discount);
            });
        }

        [Test]
        public void PrintReceipt_WithFiveProduct_ShouldReturnPrintedReceipt()
        {
            // Asseble
            var receiptService = new ReceiptService(_calculateProductPriceMock.Object);
            var receipt = receiptService.CreateReceipt(_cart);

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
