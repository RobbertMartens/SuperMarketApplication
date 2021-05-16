using Moq;
using NUnit.Framework;
using Service.Interfaces;
using Service.Models;
using Service.Services;

namespace Service.UnitTests
{
    public class CalculateCartPriceTests
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
        public void CalculatePrice_ShouldReturnCorrectDouble_WhenGivenCartNoDiscount()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0], 1)).Returns(_cart.Products[0].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[1], 1)).Returns(_cart.Products[1].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[2], 1)).Returns(_cart.Products[2].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[3], 1)).Returns(_cart.Products[3].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[4], 1)).Returns(_cart.Products[4].Price);

            var calculatePriceService = new ReceiptService(_calculateProductPriceMock.Object);

            // Assign
            var expectedPrice = 13.18;

            // Act
            var receipt = calculatePriceService.CreateReceipt(_cart);

            // Assert
            Assert.AreEqual(expectedPrice, receipt.TotalPrice);
        }
    }
}
