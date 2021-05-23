using NUnit.Framework;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System;

namespace Service.UnitTests.Services
{
    public class CalculateProductPriceTests
    {
        private ICalculateProductPrice _calculateProductPrice;

        [SetUp]
        public void SetUp()
        {
            _calculateProductPrice = new CalculateProductPrice();
        }

        [Test]
        public void CalculateProductPrice_ShouldReturnProductPrice_WhenNoDiscount()
        {
            // Assemble
            var product = new Product() { ProductName = "Kaaaas", Price = 5.49M, Discount = Discount.NoDiscount };

            // Act
            var price = _calculateProductPrice.Calculate(product, 1);

            // Assert
            Assert.AreEqual(product.Price, price);
        }

        [Test]
        public void CalculateProductPrice_ShouldReturnPriceWithBonusDiscount_WhenGivenBonusDiscount()
        {
            // Assemble 
            var product = new Product() { ProductName = "Kaaaas", Price = 5.49M, Discount = Discount.Bonus };

            // Assign
            var expectedPrice = 4.39M;

            // Act
            var price = _calculateProductPrice.Calculate(product, 1);

            // Assert
            Assert.AreEqual(expectedPrice, price);
        }

        [Test]
        public void CalculateProductPrice_ShouldReturnPriceWithExpiryDiscount_WhenGivenExpiryDiscount()
        {
            // Assemble 
            var product = new Product() { ProductName = "Kaaaas", Price = 5.49M, Discount = Discount.Expiry };

            // Assign
            var expectedPrice = 3.57M;

            // Act
            var price = _calculateProductPrice.Calculate(product, 1);

            // Assert
            Assert.AreEqual(expectedPrice, price);
        }

        [Test]
        public void CalculateProductPrice_ShouldThrowNullReferenceExpcetion_WhenGivenNullProduct()
        {
            Exception ex = Assert.Throws<NullReferenceException>(delegate
            {
                _calculateProductPrice.Calculate(null, 1);
            });
            Assert.AreEqual("given product is null!", ex.Message.ToLower());
        }
    }
}
