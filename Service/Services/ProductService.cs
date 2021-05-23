﻿using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Models;
using Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _context;

        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public async Task<int> DecreaseProductAmount(int barcode, int amount)
        {
            var product = await GetProduct(barcode);
            product.Amount -= amount;
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
        }

        public async Task<int> DeleteProduct(int barcode)
        {
            var productToDelete = await GetProduct(barcode);
            _context.Product.Remove(productToDelete);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetProduct(int barcode, bool allowChangeTracking = true)
        {
            if (allowChangeTracking)
            {
                return await _context.Product.FirstOrDefaultAsync(x => x.Barcode == barcode);
            }
            else
            {
                return await _context.Product.AsNoTracking().FirstOrDefaultAsync(x => x.Barcode == barcode);
            }
        }

        public async Task<int> IncreaseProductAmount(int barcode, int amount)
        {
            var product = await GetProduct(barcode);
            product.Amount += amount;
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
        }

        public async Task<int> InsertProduct(Product product)
        {
            await _context.Product.AddAsync(product);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected;
        }
    }
}
