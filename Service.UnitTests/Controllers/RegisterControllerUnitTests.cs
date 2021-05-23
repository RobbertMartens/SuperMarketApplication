using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Service.Controllers;
using Service.Interfaces;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.UnitTests.Controllers
{
    public class RegisterControllerUnitTests
    {
        private Mock<IRegisterService> _mockRegisterService;
        private Mock<ILogger<RegisterController>> _mockLogger;
        private RegisterController _registerController;

        private Cart _cart;

        [SetUp]
        public void Init()
        {
            _mockRegisterService = new Mock<IRegisterService>();
            _mockLogger = new Mock<ILogger<RegisterController>>();
            _cart = new Cart();
            _cart.AddToCart(new Product { ProductName = "Kaas", Barcode = 156734, Price = 4.99M, Amount = 2, Id = 1 });
            _cart.AddToCart(new Product { ProductName = "Ham", Barcode = 579843, Price = 1.49M });
            _cart.AddToCart(new Product { ProductName = "Melk", Barcode = 378941, Price = 0.99M });
            _cart.AddToCart(new Product { ProductName = "Pizza", Barcode = 739214, Price = 4.59M });
            _cart.AddToCart(new Product { ProductName = "WC papier", Barcode = 798234, Price = 1.12M });
        }

        [Test]
        public async Task PostCheckOut_CorrectCart_ShouldReturnReceipt()
        {
            // Assemble
            _mockRegisterService.Setup(mock => mock.CheckOut(_cart)).Returns(Task.FromResult("Dit is een bonnetje"));
            _registerController = new RegisterController(_mockLogger.Object, _mockRegisterService.Object);

            // Act
            var actualObjectReceipt = await _registerController.PostCheckOut(_cart);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actualObjectReceipt);
            _mockRegisterService.Verify(mock => mock.CheckOut(_cart), Times.Once);
        }

        [Test]
        public async Task PostCheckOut_NullCart_ShouldReturnBadRequest()
        {
            _cart = null;
            _mockRegisterService.Setup(mock => mock.CheckOut(_cart)).Returns(Task.FromResult("Dit is een bonnetje"));
            _registerController = new RegisterController(_mockLogger.Object, _mockRegisterService.Object);

            // Act
            var actualObjectReceipt = await _registerController.PostCheckOut(_cart);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(actualObjectReceipt);
            _mockRegisterService.Verify(mock => mock.CheckOut(_cart), Times.Never);
        }

        [Test]
        public async Task PostCheckOut_NullProducts_ShouldReturnBadRequest()
        {
            _cart.Products = null;
            _mockRegisterService.Setup(mock => mock.CheckOut(_cart)).Returns(Task.FromResult("Dit is een bonnetje"));
            _registerController = new RegisterController(_mockLogger.Object, _mockRegisterService.Object);

            // Act
            var actualObjectReceipt = await _registerController.PostCheckOut(_cart);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(actualObjectReceipt);
            _mockRegisterService.Verify(mock => mock.CheckOut(_cart), Times.Never);
        }

        [Test]
        public async Task PostCheckOut_CheckOutThrows_ShouldReturnBadRequest()
        {
            // Assemble
            _mockRegisterService.Setup(mock => mock.CheckOut(_cart)).Throws(new Exception());
            _registerController = new RegisterController(_mockLogger.Object, _mockRegisterService.Object);

            // Act
            var actualResult = await _registerController.PostCheckOut(_cart);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(actualResult);
            _mockRegisterService.Verify(mock => mock.CheckOut(_cart), Times.Once);
        }
    }
}
