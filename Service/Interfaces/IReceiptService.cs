using Service.Models;

namespace Service.Interfaces
{
    public interface IReceiptService
    {
        Receipt CreateReceipt(Cart cart);
        string PrintReceipt(Receipt receipt);
    }
}
