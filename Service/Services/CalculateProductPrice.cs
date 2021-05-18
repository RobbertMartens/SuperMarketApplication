using Service.Enum;
using Service.Interfaces;
using Service.Models;
using System;

namespace Service.Services
{
    public class CalculateProductPrice : ICalculateProductPrice
    {
        public decimal Calculate(Product product, int amount)
        {
            if (product == null)
            {
                throw new NullReferenceException("Given product is null!");
            }

            if (amount < 1)
            {
                throw new ArgumentOutOfRangeException($"Invalid amount received! Actual: {amount}");
            }

            switch (product.Discount)
            {
                case Discount.NoDiscount:
                    return Math.Round(product.Price * amount, 2);

                case Discount.Bonus:
                    return Math.Round(product.Price * amount * Constants.BonusDiscount, 2);

                case Discount.Expiry:
                    return Math.Round(product.Price * amount * Constants.ExpiryDiscount, 2);

                default:
                    throw new ArgumentOutOfRangeException($"Incorrect enum received. Actual: {product.Discount}");
            }
        }
    }
}
