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

        public async Task<int> ProcessResupplyAmounts(SupplyRequest request)
        {
            int rowsAffected = 0;

            foreach (var product in request.ProductsToSupply)
            {
                rowsAffected += await _productService.IncreaseProductAmount(product.Barcode, product.Amount);
            }
            return rowsAffected;
        }

        public async Task<SupplyRequest> GetCurrentSupplies()
        {
            var products = await _productService.GetAllProducts();
            var supplyRequest = _mapperService.MapSupplyRequest(products);
            return supplyRequest;
        }
    }
}
