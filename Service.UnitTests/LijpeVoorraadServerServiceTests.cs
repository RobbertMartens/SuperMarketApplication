﻿using Moq;
using NUnit.Framework;
using Service.Interfaces;
using Service.Models;
using Service.Services;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.UnitTests
{
    public class LijpeVoorraadServerServiceTests
    {
        private ILijpeVoorraadServerService _service;
        private Mock<ISupplyClient> _mockClient;
        private Mock<IProductService> _mockProductService;

        [Test]
        public async Task PostProvisioning_ShouldReturn200()
        {
            // Assign
            var mockResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            // Assemble
            var provisioningRequest = new SupplyRequest
            {
                ProvisionProducts = new List<ProvisioningProduct>()
            };
            provisioningRequest.ProvisionProducts.Add(new ProvisioningProduct { Amount = 5, Barcode = 123 });
            provisioningRequest.ProvisionProducts.Add(new ProvisioningProduct { Amount = 12, Barcode = 1834 });

            _mockProductService = new Mock<IProductService>();
            _mockClient = new Mock<ISupplyClient>();
            _mockClient.Setup(m => m.SendSupplyRequest(provisioningRequest)).Returns(Task.FromResult(mockResponseMessage));
            
            _service = new LijpeVoorraadServerService(_mockProductService.Object);

            // Act
            var result = await _service.PostSupplyRequest(_mockClient.Object, provisioningRequest);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            _mockClient.Verify(m => m.SendSupplyRequest(provisioningRequest), Times.Once);
        }
    }
}
