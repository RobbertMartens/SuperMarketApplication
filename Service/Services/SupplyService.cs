using Service.Interfaces;
using Service.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SupplyService : ISupplyService
    {
        private readonly IProductService _productService;
        private readonly IMapperService _mapperService;

        public SupplyService(IProductService productService, IMapperService mapperService)
        {
            _productService = productService;
            _mapperService = mapperService;
        }

        public async Task<int> ProcessResupplyAmounts(IEnumerable<Supply> supplies)
        {
            int rowsAffected = 0;

            foreach (var item in supplies)
            {
                rowsAffected += await _productService.IncreaseProductAmount(item.Barcode, item.Amount);
            }
            return rowsAffected;
        }

        public async Task<IEnumerable<Supply>> GetCurrentSupplies()
        {
            var products = await _productService.GetAllProducts();
            var supplies = _mapperService.MapSupplyRequest(products);
            return supplies;
        }
    }
}
