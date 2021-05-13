using Service.Models;

namespace Service.Interfaces
{
    public interface ICalculateProductPrice
    {
        decimal Calculate(Product product);
    }
}
