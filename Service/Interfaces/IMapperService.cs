using Service.Models;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IMapperService
    {
        Receipt MapReceipt(IEnumerable<ReceiptProduct> receiptProducts);
        ReceiptProduct MapReceiptProduct(Product product);
        IEnumerable<Supply> MapSupplyRequest(IEnumerable<Product> products);
    }
}
