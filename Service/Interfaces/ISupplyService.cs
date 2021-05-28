using Service.Models;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISupplyService
    {
        Task <int> ProcessResupplyAmounts(SupplyRequest request);
        Task<SupplyRequest> GetCurrentSupplies();
    }
}
