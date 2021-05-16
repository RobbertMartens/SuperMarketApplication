using Moq;
using NUnit.Framework;
using Service.Interfaces;
using Service.Models;
using Service.Services;

namespace Service.UnitTests
{
    public class RegisterServiceTests
    {
        private Mock<IReceiptService> _calculatePriceServiceMock;
        private Cart _cart;

        [SetUp]
        public void Setup()
        {
            _calculatePriceServiceMock = new Mock<IReceiptService>();
            
            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M });
        }

        [Test]
        public void Register_ShouldPass_WhenCheckOut()
        {
            // Assemble
            _calculatePriceServiceMock.Setup(mock => mock.CreateReceipt(_cart));
            var registerService = new RegisterService(_calculatePriceServiceMock.Object);

            // Act
            registerService.CheckOut(_cart);

            // Assert
            _calculatePriceServiceMock.Verify(mock => mock.CreateReceipt(_cart), Times.Once);
        }
    }
}