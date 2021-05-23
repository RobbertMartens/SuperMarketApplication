using Moq;
using NUnit.Framework;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.UnitTests.Services
{
    public class LijpeVoorraadServerServiceTests
    {
        private LijpeVoorraadServerService _voorraadService;
        private Mock<IProductService> _mockProductService;
        private Mock<IMapperService> _mockMapperService;

        [SetUp]
        public void Init()
        {
            _mockProductService = new Mock<IProductService>();
            _mockMapperService = new Mock<IMapperService>();
        }

        [Test]
        public async Task ProcessResupplyAmounts_ShouldReturnResupplyRequest()
        {
            // Assemble
            _mockProductService.Setup(mock => mock.IncreaseProductAmount(It.IsAny<int>(), It.IsAny<int>())).
                Returns(Task.FromResult(1));
            _voorraadService = new LijpeVoorraadServerService(_mockProductService.Object, _mockMapperService.Object);
            var supplyRequest = new SupplyRequest
            {
                ProductsToSupply = new List<ProductToSupply>
            {
                    new ProductToSupply
                    {
                        Barcode = 123,
                        Amount = 5
                    },
                    new ProductToSupply
                    {
                        Barcode = 5345,
                        Amount = 3
                    }
                }
            };

            // Assign
            var expectedRowsAffected = 2;

            // Act
            var rowsAffected = await _voorraadService.ProcessResupplyAmounts(supplyRequest);

            // Assert
            Assert.AreEqual(expectedRowsAffected, rowsAffected);
            _mockProductService.Verify(mock => mock.IncreaseProductAmount(It.IsAny<int>(), It.IsAny<int>()), 
                Times.Exactly(2));
        }

        [Test]
        public async Task GetCurrentSupplies_ShouldReturnSupplyRequest()
        {
            // Assemble
            IEnumerable<Product> products = new List<Product>
            {
                new Product
                {
                    Amount = 50,
                    Barcode = 123,
                    Discount = Discount.NoDiscount,
                    Id = 5,
                    Price = 4.99M,
                    ProductName = "Kaas"
                },
                new Product
                {
                    Amount = 100,
                    Barcode = 321,
                    Discount = Discount.Expiry,
                    Id = 10,
                    Price = 14.99M,
                    ProductName = "Bier"
                }
            };

            var supplyRequest = new SupplyRequest
            {
                ProductsToSupply = new List<ProductToSupply>
                {
                    new ProductToSupply
                    {
                        Amount = 50,
                        Barcode = 123
                    },
                    new ProductToSupply
                    {
                        Amount = 0,
                        Barcode = 321
                    }
                }
            };

            _mockProductService.Setup(mock => mock.GetAllProducts()).Returns(Task.FromResult(products));
            _mockMapperService.Setup(mock => mock.MapSupplyRequest(products)).Returns(supplyRequest);
            _voorraadService = new LijpeVoorraadServerService(_mockProductService.Object, _mockMapperService.Object);

            // Act
            var actualSupplyRequest = await _voorraadService.GetCurrentSupplies();

            // Assert
            _mockProductService.Verify(mock => mock.GetAllProducts(), Times.Once);
            _mockMapperService.Verify(mock => mock.MapSupplyRequest(products), Times.Once);
        }
    }
}
