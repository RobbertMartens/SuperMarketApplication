using Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Models
{
    public class Receipt
    {
        public string Message { get; set; } = "Welkom bij de Boni M! Thx voor je shit!";
        public DateTime TimePrinted { get; set; }
        public List<ProductReceipt> BoughtProducts { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class ProductReceipt
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public int Barcode { get; set; }
        public Discount Discount { get; set; } = Discount.NoDiscount;
        public decimal ProductPrice { get; set; }
        public decimal ProductPriceWithDiscount { get; set; }
        public decimal Total { get; set; }
    }
}
