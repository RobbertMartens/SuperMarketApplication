using Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISupplyService
    {
        Task <int> ProcessResupplyAmounts(IEnumerable<Supply> supplies);
        Task<IEnumerable<Supply>> GetCurrentSupplies();
    }
}
