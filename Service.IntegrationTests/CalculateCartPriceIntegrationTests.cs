using NUnit.Framework;
using Service.Enum;
using Service.Models;

namespace Service.IntegrationTests
{
    public class CalculateCartPriceIntegrationTests : Init
    {
        private Cart _cart;

        [SetUp]
        public void SetUp()
        {
            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M });
        }

        [Test]
        public void CalculateCart_NoDiscount_ShouldBeCorrectPrice()
        {
            // Act
            var price = CalculateCartPrice.Calculate(_cart);

            // Assert
            Assert.AreEqual(13.18, price);
        }

        [Test]
        public void CalculateCart_WithBonusAndExpiryDiscount_ShouldBeCorrectPrice()
        {
            _cart.Products[0].Discount = Discount.Bonus;
            _cart.Products[2].Discount = Discount.Expiry;
            _cart.Products[4].Discount = Discount.Bonus;

            // Act
            var price = CalculateCartPrice.Calculate(_cart);

            // Assert
            Assert.AreEqual(11.61, price);
        }

        [Test]
        public void CartWithProduct_CheckOut_ShouldPrintCorrectReceipt()
        {
            // Assign
            var expectedPrice = 13.18;

            // Act
            var receipt = RegisterService.CheckOut(_cart);

            // Assert
            Assert.That(receipt.Contains(expectedPrice.ToString()));
        }
    }
}