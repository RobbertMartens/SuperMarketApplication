using Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        [Required()]
        public int Barcode { get; set; }
        public decimal Price { get; set; }
        public Discount Discount { get; set; } = Discount.NoDiscount;
        [Required()]
        public int Amount { get; set; }
    }
}
