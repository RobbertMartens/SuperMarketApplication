using NUnit.Framework;
using Service.Clients;
using System.Net;
using System.Threading.Tasks;

namespace Service.IntegrationTests
{
    public class LijpeVoorraadServerIntegrationTests : Init
    {
        [Test]
        public async Task GetProvisionProductsTest_ShouldReturnAtLeastTwoProducts()
        {
            // Act
            var provisionProducts = await LijpeVoorraadServerService.CreateSupplyRequest(100);

            // Assert
            Assert.That(provisionProducts.ProvisionProducts.Count >= 2);
        }

        [Test]
        public async Task PostProvisionRequest_ShouldPass()
        {
            // Assign
            var expectedStatusCode = HttpStatusCode.OK;

            // Assemble
            var provisionRequest = await LijpeVoorraadServerService .CreateSupplyRequest(100);
            var provisioningClient = new LijpeVoorraadServerClient();

            // Act
            var result = await LijpeVoorraadServerService.PostSupplyRequest(provisioningClient, provisionRequest);

            // Assert
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }
    }
}
