using Service.Models;

namespace Service.Interfaces
{
    public interface ICalculateCartPrice
    {
        decimal Calculate(Cart cart);
    }
}
