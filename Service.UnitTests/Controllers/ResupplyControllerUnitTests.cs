﻿using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Service.Controllers;
using Service.Enum;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.UnitTests.Controllers
{
    public class ResupplyControllerUnitTests
    {
        private Mock<ILogger<ResupplyController>> _mockLogger;
        private Mock<ILijpeVoorraadServerService> _mockVoorraadservice;
        private Mock<IProductService> _mockProductService;
        private ResupplyController _resupplyController;

        [SetUp]
        public void Init()
        {
            _mockLogger = new Mock<ILogger<ResupplyController>>();
            _mockVoorraadservice = new Mock<ILijpeVoorraadServerService>();
            _mockProductService = new Mock<IProductService>();
        }

        [Test]
        public async Task PutSupply_WithCorrectArguments_ShouldReturnOkObjectResult()
        {
            // Assemble
            var resupplyRequest = new SupplyRequest
            {
                ProductsToSupply = new List<ProductToSupply>
                {
                    new ProductToSupply
                    {
                        Amount = 5,
                        Barcode = 123
                    },
                    new ProductToSupply
                    {
                        Amount = 10,
                        Barcode = 321
                    }
                }
            };
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(resupplyRequest));

            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _resupplyController.PutResupply(resupplyRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(resupplyRequest), Times.Once);
        }

        [Test]
        public async Task PutSupply_WithNullResupplyRequest_ShouldReturnBadRequestResult()
        {
            // Assemble
            SupplyRequest resupplyRequest = null;
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(resupplyRequest));

            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _resupplyController.PutResupply(resupplyRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(resupplyRequest), Times.Never);
        }

        [Test]
        public async Task PutSupply_WithNullProductsToResupply_ShouldReturnBadRequestResult()
        {
            // Assemble
            SupplyRequest resupplyRequest = new SupplyRequest
            {
                ProductsToSupply = null
            };
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(resupplyRequest));

            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _resupplyController.PutResupply(resupplyRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(resupplyRequest), Times.Never);
        }

        [Test]
        public async Task PutSupply_VoorraadServiceThrows_ShouldReturnBadRequestResult()
        {
            // Assemble
            var resupplyRequest = new SupplyRequest
            {
                ProductsToSupply = new List<ProductToSupply>
                {
                    new ProductToSupply
                    {
                        Amount = 5,
                        Barcode = 123
                    },
                    new ProductToSupply
                    {
                        Amount = 10,
                        Barcode = 321
                    }
                }
            };
            _mockVoorraadservice.Setup(mock => mock.ProcessResupplyAmounts(resupplyRequest)).Throws(new Exception());

            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var objectResult = await _resupplyController.PutResupply(resupplyRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(objectResult);
            _mockVoorraadservice.Verify(mock => mock.ProcessResupplyAmounts(resupplyRequest), Times.Once);
        }

        [Test]
        public async Task GetCurrentSupplies_ShouldReturnOkObjectResult()
        {
            // Assemble
            var resupplyRequest = new SupplyRequest
            {
                ProductsToSupply = new List<ProductToSupply>
                {
                    new ProductToSupply
                    {
                        Amount = 5,
                        Barcode = 123
                    },
                    new ProductToSupply
                    {
                        Amount = 10,
                        Barcode = 321
                    }
                }
            };
            _mockVoorraadservice.Setup(mock => mock.GetCurrentSupplies()).Returns(Task.FromResult(resupplyRequest));
            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object, 
                _mockProductService.Object);

            // Act
            var result = await _resupplyController.GetCurrentSupplies();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            _mockVoorraadservice.Verify(mock => mock.GetCurrentSupplies(), Times.Once);
        }

        [Test]
        public async Task GetCurrentSupplies_WhenGetSuppliesThrows_ShouldReturnBadRequest()
        {
            // Assemble
            _mockVoorraadservice.Setup(mock => mock.GetCurrentSupplies()).Throws(new Exception());
            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _resupplyController.GetCurrentSupplies();

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
            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _resupplyController.PostNewProduct(product);

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
            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _resupplyController.PostNewProduct(product);

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
            _resupplyController = new ResupplyController(_mockLogger.Object, _mockVoorraadservice.Object,
                _mockProductService.Object);

            // Act
            var result = await _resupplyController.PostNewProduct(product);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
            _mockProductService.Verify(mock => mock.InsertProduct(product), Times.Once);
        }
    }
}