using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class SupplyRequest
    {
        public List<ProductToSupply> ProductsToSupply { get; set; }
    }

    public class ProductToSupply
    {
        public int Barcode { get; set; }
        public int Amount { get; set; }
    }
}
