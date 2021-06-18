using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class Cart
    {
        [Required()]
        public List<Product> Products { get; set; } = new List<Product>();

        public void AddToCart(Product product) => Products.Add(product);
    }
}
