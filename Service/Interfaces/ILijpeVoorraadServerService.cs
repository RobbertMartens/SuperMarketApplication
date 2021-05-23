using Service.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ILijpeVoorraadServerService
    {
        Task <int> ProcessResupplyAmounts(SupplyRequest request);
        Task<SupplyRequest> GetCurrentSupplies();
    }
}
