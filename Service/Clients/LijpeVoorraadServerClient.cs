using Newtonsoft.Json;
using Service.Interfaces;
using Service.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Clients
{
    public class LijpeVoorraadServerClient : ISupplyClient
    {
        public async Task<HttpResponseMessage> SendSupplyRequest(SupplyRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await new HttpClient().PostAsync("https://reqbin.com/echo/post/json", body);

            return result;
        }
    }
}
