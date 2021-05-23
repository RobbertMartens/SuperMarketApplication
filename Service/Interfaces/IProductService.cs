using Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<int> DecreaseProductAmount(int barcode, int amount);
        Task<int> DeleteProduct(int barcode);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(int barcode, bool allowChangeTracking = true);
        Task<int> InsertProduct(Product product);
        Task<int> IncreaseProductAmount(int barcode, int amount);
    }
}
