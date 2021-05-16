using Newtonsoft.Json;
using NUnit.Framework;
using Service.Enum;
using Service.Models;

namespace Service.IntegrationTests
{
    public class ReceiptServiceIntegrationTests : Init
    {
        private Cart _cart;

        [SetUp]
        public void SetUp()
        {
            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M, Amount = 1 });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M, Amount = 1 });
        }

        [Test]
        public void CreateReceipt_NoDiscount_ShouldBeCorrectPrice()
        {
            // Act
            var receipt = ReceiptService.CreateReceipt(_cart);

            // Assert
            Assert.AreEqual(13.18M, receipt.TotalPrice);
        }

        [Test]
        public void CreateReceipt_WithBonusAndExpiryDiscount_ShouldBeCorrectPrice()
        {
            _cart.Products[0].Discount = Discount.Bonus;
            _cart.Products[2].Discount = Discount.Expiry;
            _cart.Products[4].Discount = Discount.Bonus;

            // Act
            var receipt = ReceiptService.CreateReceipt(_cart);

            // Assert
            Assert.AreEqual(11.61M, receipt.TotalPrice);
        }

        [Test]
        public void CreateReceipt_MultipleAmountsAndDiscount_ShouldBeCorrectPrice()
        {
            // Assemble
            _cart.Products[0].Discount = Discount.Bonus;
            _cart.Products[2].Discount = Discount.Expiry;
            _cart.Products[4].Discount = Discount.Bonus;

            _cart.Products[0].Amount = 3;
            _cart.Products[2].Amount = 2;
            _cart.Products[4].Amount = 3;

            // Assign
            var expectedTotal = 22.04M;

            // Act
            var receipt = ReceiptService.CreateReceipt(_cart);

            // Assert
            Assert.AreEqual(expectedTotal, receipt.TotalPrice);
        }

        [Test]
        public void CreateReceipt_CheckProperties_ShouldReturnCorrectValueProperties()
        {
            // Assemble
            _cart.Products[0].Discount = Discount.Bonus;
            _cart.Products[0].Amount = 3;

            // Assign
            var expectedMessage = "Bedankt dat u bij de Boni bent geweest!";            
            var expectedProductName = "Kaas";
            var expectedOriginalPrice = 4.99M;
            var expectedDiscount = Discount.Bonus;
            var expectedDiscountPrice = 3.99M;
            var expectedTotal = 11.98M;

            // Act
            var receipt = ReceiptService.CreateReceipt(_cart);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedMessage, receipt.Message);
                Assert.AreEqual(expectedProductName, receipt.BoughtProducts[0].ProductName);
                Assert.AreEqual(expectedOriginalPrice, receipt.BoughtProducts[0].ProductPrice);
                Assert.AreEqual(expectedDiscount, receipt.BoughtProducts[0].Discount);
                Assert.AreEqual(expectedDiscountPrice, receipt.BoughtProducts[0].ProductPriceWithDiscount);
                Assert.AreEqual(expectedTotal, receipt.BoughtProducts[0].Total);
            });
        }
    }
}