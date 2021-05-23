using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Service.Interfaces;
using Service.Models;
using Service.Repositories;

namespace Service.UnitTests.Services
{
    public class ProductServiceTests
    {
        private Mock<DbSet<Product>> _mockSet;
        private Mock<ProductContext> _mockContext;
        private IProductService _productService;

        [SetUp]
        public void Init()
        {
            _mockSet = new Mock<DbSet<Product>>();
            _mockContext = new Mock<ProductContext>();
        }

        //[Test]
        //public void AddProductTest_ShouldReturnOneRowAffected()
        //{
        //    // Assemble
        //    var product = new ProductDB
        //    {
        //        ProductName = "Test product",
        //        Barcode = 12345,
        //        Price = 1.99M,
        //        Discount = Discount.NoDiscount,
        //        Amount = 1
        //    };

        //    _mockContext.Setup(m => m.Product).Returns(() =>_mockSet.Object);
        //    _productService = new ProductService(_mockContext.Object);

        //    // Act
        //    _productService.InsertProduct(product);

        //    // Assert
        //    _mockSet.Verify(m => m.Add(It.IsAny<ProductDB>()), Times.Once);
        //    _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        //}
    }
}
