using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using Service.Repositories;
using Service.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Service.UnitTests.Services
{
    public class ProductServiceTests
    {
        private Mock<DbSet<Product>> _mockSet;
        private Mock<ProductContext> _mockContext;
        private Mock<DbContextOptions> _mockDbContextOptions;
        private IProductService _productService;

        [SetUp]
        public void Init()
        {
            _mockDbContextOptions = new Mock<DbContextOptions>(It.IsAny<IReadOnlyDictionary<It.IsAnyType, It.IsAnyType>>());
            _mockSet = new Mock<DbSet<Product>>();
            _mockContext = new Mock<ProductContext>(new DbContextOptionsBuilder().Options);
            
        }

        [Test]
        public async Task AddProductTest_ShouldReturnOneRowAffected()
        {
            // Assemble
            var product = new Product
            {
                ProductName = "Test product",
                Barcode = 12345,
                Price = 1.99M,
                Discount = Discount.NoDiscount,
                Amount = 1
            };

            _mockContext.Setup(m => m.Product).Returns(() => _mockSet.Object);
            _productService = new ProductService(_mockContext.Object);

            // Act
            await _productService.InsertProduct(product);

            // Assert
            _mockSet.Verify(mock => mock.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
