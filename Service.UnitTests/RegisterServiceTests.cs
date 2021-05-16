using Moq;
using NUnit.Framework;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.UnitTests
{
    public class RegisterServiceTests
    {
        private Mock<IReceiptService> _calculatePriceServiceMock;
        private Mock<IProductService> _productServiceMock;
        private Cart _cart;

        [SetUp]
        public void Setup()
        {
            _calculatePriceServiceMock = new Mock<IReceiptService>();
            _productServiceMock = new Mock<IProductService>();

            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M });
        }

        [Test]
        public async Task Register_ShouldPass_WhenCheckOut()
        {
            // Assemble
            _calculatePriceServiceMock.Setup(mock => mock.CreateReceipt(_cart)).Returns(new Receipt
            {
                Message = "hoi test",
                TotalPrice = 1.00M,
                TimePrinted = DateTime.Now,
                BoughtProducts = new List<ProductReceipt>
                {
                    new ProductReceipt
                    {
                        ProductName = _cart.Products[0].ProductName,
                        Barcode = _cart.Products[0].Barcode,
                        ProductPrice = _cart.Products[0].Price
                    },
                    new ProductReceipt
                    {
                        ProductName = _cart.Products[1].ProductName,
                        Barcode = _cart.Products[1].Barcode,
                        ProductPrice = _cart.Products[1].Price
                    }
                }
            });

            var registerService = new RegisterService(_calculatePriceServiceMock.Object, _productServiceMock.Object);

            // Act
            await registerService.CheckOut(_cart);

            // Assert
            _calculatePriceServiceMock.Verify(mock => mock.CreateReceipt(_cart), Times.Once);
            _productServiceMock.Verify(mock => mock.DecreaseProductAmount(It.IsAny<int>(), It.IsAny<int>()), 
                Times.Exactly(2));
        }
    }
}