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
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M });
        }

        [Test]
        public void CalculatePrice_ShouldReturnCorrectDouble_WhenGivenCartNoDiscount()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0])).Returns(_cart.Products[0].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[1])).Returns(_cart.Products[1].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[2])).Returns(_cart.Products[2].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[3])).Returns(_cart.Products[3].Price);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[4])).Returns(_cart.Products[4].Price);

            var calculatePriceService = new CalculateCartPrice(_calculateProductPriceMock.Object);

            // Assign
            var expectedPrice = 13.18;

            // Act
            var price = calculatePriceService.Calculate(_cart);

            // Assert
            Assert.AreEqual(expectedPrice, price);
        }

        [Test]
        public void CalculatePrice_ShouldRoundUp_WhenGivenThreeDecimals()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0])).Returns(5.495M);
            var calculatePriceService = new CalculateCartPrice(_calculateProductPriceMock.Object);

            // Assign
            var expectedPrice = 5.50;

            // Act
            var price = calculatePriceService.Calculate(_cart);

            // Assert
            Assert.AreEqual(expectedPrice, price);
        }

        [Test]
        public void CalculatePrice_ShouldRoundDown_WhenGivenThreeDecimals()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0])).Returns(5.494M);
            var calculatePriceService = new CalculateCartPrice(_calculateProductPriceMock.Object);

            // Assign
            var expectedPrice = 5.49;

            // Act
            var price = calculatePriceService.Calculate(_cart);

            // Assert
            Assert.AreEqual(expectedPrice, price);
        }
    }
}
