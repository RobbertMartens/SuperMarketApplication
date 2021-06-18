using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Service.Controllers;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UnitTests.Controllers
{
    public class SupplyControllerUnitTests
    {
        private Mock<ILogger<SupplyController>> _mockLogger;
        private Mock<ISupplyService> _mockVoorraadservice;
        private Mock<IProductService> _mockProductService;
        private SupplyController _supplyController;

        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILogger<SupplyController>>();
            _mockVoorraadservice = new Mock<ISupplyService>();
            _mockProductService = new Mock<IProductService>();
        }

        [Test]
        public async Task PutSupply_WithCorrectArguments_ShouldReturnOkObjectResult()
        {
            // Assemble
            var supplies = new List<Supply>
            {
                new Supply
                {
                        Amount = 5,
                        Barcode = 123
                },
                new Supply
                {
                        Amount = 10,
                        Barcode = 321
                }
            };

            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(supplies));

            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _supplyController.PutSupply(supplies);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(supplies), Times.Once);
        }

        [Test]
        public async Task PutSupply_WithNullResupplyRequest_ShouldReturnBadRequestResult()
        {
            // Assemble
            List<Supply> supplyRequest = null;
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(supplyRequest));

            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _supplyController.PutSupply(supplyRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(supplyRequest), Times.Never);
        }

        [Test]
        public async Task PutSupply_WithNullProductsToResupply_ShouldReturnBadRequestResult()
        {
            // Assemble
            var supplies = new List<Supply>();
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(supplies));

            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _supplyController.PutSupply(supplies);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(supplies), Times.Never);
        }

        [Test]
        public async Task PutSupply_VoorraadServiceThrows_ShouldReturnBadRequestResult()
        {
            // Assemble
            var supplies = new List<Supply>
            {
                new Supply
                {
                    Amount = 5,
                    Barcode = 123
                },
                new Supply
                {
                    Amount = 10,
                    Barcode = 321
                }
            };
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(supplies)).Throws(new Exception());

            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _supplyController.PutSupply(supplies);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(supplies), Times.Once);
        }

        [Test]
        public async Task GetCurrentSupplies_ShouldReturnOkObjectResult()
        {
            // Assemble
            IEnumerable<Supply> supplies = new List<Supply>();
            supplies.Append(new Supply
            {
                Amount = 5,
                Barcode = 123
            });
            supplies.Append(new Supply
            {
                Amount = 10,
                Barcode = 321
            });

            _mockVoorraadservice.Setup(mock => mock.GetCurrentSupplies()).Returns(Task.FromResult(supplies));
            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object, 
                _mockProductService.Object);

            // Act
            var result = await _supplyController.GetCurrentSupplies();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            _mockVoorraadservice.Verify(mock => mock.GetCurrentSupplies(), Times.Once);
        }

        [Test]
        public async Task GetCurrentSupplies_WhenGetSuppliesThrows_ShouldReturnBadRequest()
        {
            // Assemble
            _mockVoorraadservice.Setup(mock => mock.GetCurrentSupplies()).Throws(new Exception());
            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _supplyController.GetCurrentSupplies();

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            _mockVoorraadservice.Verify(mock => mock.GetCurrentSupplies(), Times.Once);
        }

        [Test]
        public async Task PostProduct_WithCorrectProduct_ShouldReturnOkResult()
        {
            // Assemble
            var product = new Product
            {
                Amount = 100,
                Barcode = 123,
                Discount = Discount.NoDiscount,
                Price = 5.99M,
                ProductName = "Kaas"
            };

            _mockProductService.Setup(mock => mock.InsertProduct(product));
            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _supplyController.PostNewProduct(product);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _mockProductService.Verify(mock => mock.InsertProduct(product), Times.Once);
        }

        [Test]
        public async Task PostProduct_WithNullProduct_ShouldReturnBadRequest()
        {
            // Assemble
            Product product = null;

            _mockProductService.Setup(mock => mock.InsertProduct(product));
            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _supplyController.PostNewProduct(product);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            _mockProductService.Verify(mock => mock.InsertProduct(product), Times.Never);
        }

        [Test]
        public async Task PostProduct_WhenPostThrows_ShouldReturnBadRequest()
        {
            // Assemble
            var product = new Product
            {
                Amount = 100,
                Barcode = 123,
                Discount = Discount.NoDiscount,
                Price = 5.99M,
                ProductName = "Kaas"
            };

            _mockProductService.Setup(mock => mock.InsertProduct(product)).ThrowsAsync(new Exception());
            _supplyController = new SupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _supplyController.PostNewProduct(product);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            _mockProductService.Verify(mock => mock.InsertProduct(product), Times.Once);
        }
    }
}
