using Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<int> DecreaseProductAmount(int barcode, int amount);
        Task<int> DeleteProduct(int barcode);
        Task<Product> GetProduct(int barcode);
        Task<IEnumerable<Product>> GetProductsToResupply(int supplyMax);
        Task<int> InsertProduct(Product product);
        Task<int> IncreaseProductAmount(int barcode, int amount);
    }
}
