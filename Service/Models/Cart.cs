using System.Collections.Generic;

namespace Service.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; } = new List<Product>();

        public void AddToCart(Product product) => Products.Add(product);
    }
}
