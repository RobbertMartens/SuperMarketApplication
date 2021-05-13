using NUnit.Framework;
using Service.Enum;
using Service.IntegrationTests;
using Service.Models;
using System;
using System.Threading.Tasks;

namespace Service.IntegrationTests
{
    public class ProductServiceIntegrationTests : Init
    {
        [Test]
        public async Task DecreaseProductAmountByOne_ShouldReturnOneRowAffected()
        {
            // Assign
            var expectedRowsAffected = 1;

            // Assemble
            var barcode = 156734;

            // Act
            var rowsAffected = await ProductService.DecreaseProductAmount(barcode, 1);

            // Assert
            Assert.AreEqual(expectedRowsAffected, rowsAffected);
        }

        [Test]
        public async Task DeleteProductTest_ShouldReturnOneRowAffected()
        {
            // Assign
            var barcode = 54752848;

            // Assemble
            var product = new Product
            {
                Amount = 100,
                Barcode = barcode,
                Discount = Discount.NoDiscount,
                Price = 1.59M,
                ProductName = "Should not be visible in DB"
            };

            await ProductService.InsertProduct(product);

            // Act
            var rowsAffected = await ProductService.DeleteProduct(barcode);

            // Assert
            Assert.AreEqual(1, rowsAffected);
        }

        [Test]
        public async Task GetProductTest_ShouldReturnOneProduct()
        {
            var product = await ProductService.GetProduct(156734);
            Assert.AreEqual("Kaas", product.ProductName);
        }

        [Test]
        public async Task InsertProductTest_ShouldReturnOneRowAffectedWhenProductAdded()
        {
            // Assign 
            var expectedRowsAffected = 1;

            // Assemble
            var barcode = 23564234;

            var product = new Product
            {
                ProductName = "Bananen",
                Barcode = barcode,
                Price = 1.99M,
                Discount = Discount.Bonus,
                Amount = 100
            };

            var actualRowsAffected = await ProductService.InsertProduct(product);

            // Assert
            Assert.AreEqual(expectedRowsAffected, actualRowsAffected);

            // Clean up
            await CleanUp(barcode);
        }

        private async Task CleanUp(int barcode)
        {
            try
            {
                await ProductService.DeleteProduct(barcode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during clean up! Product with barcode {barcode} is NOT deleted");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }
    }
}