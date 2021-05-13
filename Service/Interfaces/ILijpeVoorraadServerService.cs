using Service.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ILijpeVoorraadServerService
    {
        Task<HttpResponseMessage> PostSupplyRequest(ISupplyClient client, SupplyRequest request);
        Task <int> ProcessResupplyAmounts(SupplyRequest request);
        Task<SupplyRequest> CreateSupplyRequest(int provisionMax);
    }
}
