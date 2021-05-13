using Service.Enum;
using Service.Interfaces;
using Service.Models;
using System;

namespace Service.Services
{
    public class CalculateProductPrice : ICalculateProductPrice
    {
        public decimal Calculate(Product product)
        {
            if (product == null)
            {
                throw new NullReferenceException("Given product is null!");
            }

            switch (product.Discount)
            {
                case Discount.NoDiscount:
                    return product.Price;

                case Discount.Bonus:
                    return product.Price * Constants.BonusDiscount;

                case Discount.Expiry:
                    return product.Price * Constants.ExpiryDiscount;

                default:
                    throw new ArgumentOutOfRangeException($"Incorrect enum received. Actual: {product.Discount}");
            }
        }
    }
}
