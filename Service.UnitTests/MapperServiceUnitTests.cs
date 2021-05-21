using Moq;
using NUnit.Framework;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Service.UnitTests
{
    public class MapperServiceUnitTests
    {
        private IMapperService _mapperService;
        private Mock<ICalculateProductPrice> _calculateProductPriceMock;
        private Cart _cart;

        [SetUp]
        public void SetUp()
        {
            _calculateProductPriceMock = new Mock<ICalculateProductPrice>();

            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M , Amount = 2, Id = 1});
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M });
        }

        [Test]
        public void MapReceiptProduct_CheckAllProperties_ShouldReturnCorrectProperties()
        {
            // Assemble
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0], _cart.Products[0].Amount)).Returns(9.98M);
            _calculateProductPriceMock.Setup(mock => mock.Calculate(_cart.Products[0], 1)).Returns(4.99M);
            _mapperService = new MapperService(_calculateProductPriceMock.Object);

            // Act
            var receiptProduct = _mapperService.MapReceiptProduct(_cart.Products[0]);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Kaas", receiptProduct.ProductName);
                Assert.AreEqual(156734, receiptProduct.Barcode);
                Assert.AreEqual(2, receiptProduct.Amount);
                Assert.AreEqual(Discount.NoDiscount, receiptProduct.Discount);
                Assert.AreEqual(4.99M, receiptProduct.ProductPrice);
                Assert.AreEqual(4.99M, receiptProduct.ProductPriceWithDiscount);
                Assert.AreEqual(9.98M, receiptProduct.Total);
            });
            _calculateProductPriceMock.Verify(mock => mock.Calculate(_cart.Products[0], _cart.Products[0].Amount), Times.Exactly(1));
            _calculateProductPriceMock.Verify(mock => mock.Calculate(_cart.Products[0], 1), Times.Exactly(1));
        }

        [Test]
        public void MapReceiptProduct_PassNullValue_ShouldThrowNullReferenceException()
        {
            // Assemble
            Product product = null;
            _mapperService = new MapperService(_calculateProductPriceMock.Object);

            // Act & Assert
            Assert.That(() => _mapperService.MapReceiptProduct(product), Throws.TypeOf<NullReferenceException>());
            _calculateProductPriceMock.Verify(mock => mock.Calculate(It.IsAny<Product>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void MapReceipt_CheckAllProperties_ShouldReturnCorrectProperties()
        {
            // Assemble
            var receiptProducts = new List<ReceiptProduct> { new ReceiptProduct {Total = 4.99M}, 
                new ReceiptProduct{Total = 2.02M}};
            
            _mapperService = new MapperService(_calculateProductPriceMock.Object);

            // Act
            var receipt = _mapperService.MapReceipt(receiptProducts);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Bedankt dat u bij de Boni bent geweest!", receipt.Message);
                Assert.NotNull(receipt.TimePrinted);
                Assert.AreEqual(receipt.BoughtProducts.Count, 2);
                Assert.AreEqual(7.01M, receipt.TotalPrice);
            });
        }

        [Test]
        public void MapReceipt_PassNullValue_ShouldThrowNullReferenceException()
        {
            // Assemble
            IEnumerable<ReceiptProduct> receiptProducts = null;
            _mapperService = new MapperService(_calculateProductPriceMock.Object);

            // Act & Assert
            Assert.That(() => _mapperService.MapReceipt(receiptProducts), Throws.TypeOf<NullReferenceException>());
        }
    }
}
