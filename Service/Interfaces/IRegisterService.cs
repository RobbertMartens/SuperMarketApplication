using Service.Models;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IRegisterService
    {
        Task<string> CheckOut(Cart cart);
    }
}
