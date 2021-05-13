using Service.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISupplyClient
    {
        Task<HttpResponseMessage> SendSupplyRequest(SupplyRequest request);
    }
}
