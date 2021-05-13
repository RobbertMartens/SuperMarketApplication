using Service.Interfaces;
using Service.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Services
{
    public class LijpeVoorraadServerService : ILijpeVoorraadServerService
    {
        private readonly IProductService _productService;

        public LijpeVoorraadServerService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<HttpResponseMessage> PostSupplyRequest(ISupplyClient client, SupplyRequest request)
        {
            return await client.SendSupplyRequest(request);
        }

        public async Task<int> ProcessResupplyAmounts(SupplyRequest request)
        {
            int rowsAffected = 0;

            foreach (var product in request.ProvisionProducts)
            {
                rowsAffected += await _productService.IncreaseProductAmount(product.Barcode, product.Amount);
            }
            return rowsAffected;
        }

        public async Task<SupplyRequest> CreateSupplyRequest(int provisionMax)
        {
            var provisioningRequest = new SupplyRequest
            {
                ProvisionProducts = new List<ProvisioningProduct>()
            };

            var provisioningProducts = await _productService.GetProductsToResupply(100);

            foreach (var product in provisioningProducts)
            {
                provisioningRequest.ProvisionProducts.Add(new ProvisioningProduct
                {
                    Barcode = product.Barcode,
                    Amount = provisionMax - product.Amount
                });
            }
            return provisioningRequest;
        }
    }
}
