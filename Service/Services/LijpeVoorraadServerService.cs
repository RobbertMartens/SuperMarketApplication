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
        private readonly IMapperService _mapperService;

        public LijpeVoorraadServerService(IProductService productService, IMapperService mapperService)
        {
            _productService = productService;
            _mapperService = mapperService;
        }

        public async Task<HttpResponseMessage> PostSupplyRequest(ISupplyClient client, SupplyRequest request)
        {
            return await client.SendSupplyRequest(request);
        }

        public async Task<int> ProcessResupplyAmounts(SupplyRequest request)
        {
            int rowsAffected = 0;

            foreach (var product in request.ProductsToSupply)
            {
                rowsAffected += await _productService.IncreaseProductAmount(product.Barcode, product.Amount);
            }
            return rowsAffected;
        }

        public async Task<SupplyRequest> CreateSupplyRequest(int supplyMax)
        {
            var supplyRequest = new SupplyRequest
            {
                ProductsToSupply = new List<ProductToSupply>()
            };

            var provisioningProducts = await _productService.GetProductsToResupply(100);

            foreach (var product in provisioningProducts)
            {
                supplyRequest.ProductsToSupply.Add(new ProductToSupply
                {
                    Barcode = product.Barcode,
                    Amount = supplyMax - product.Amount
                });
            }
            return supplyRequest;
        }

        public async Task<SupplyRequest> GetCurrentSupplies()
        {
            var products = await _productService.GetAllProducts();
            var supplyRequest = _mapperService.MapSupplyRequest(products);
            return supplyRequest;
        }
    }
}
