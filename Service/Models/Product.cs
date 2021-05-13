using Service.Enum;

namespace Service.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Barcode { get; set; }
        public decimal Price { get; set; }
        public Discount Discount { get; set; } = Discount.NoDiscount;
        public int Amount { get; set; }
    }
}
