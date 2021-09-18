using MyEshop.Data.Context;
using MyEshop.Domain.Interfaces;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyEshopDBContext _dbContext;

        public ProductRepository(MyEshopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAsyncEnumerable<Product> GetProductsAllAsync() => _dbContext.Products;

        public ValueTask<Product> GetProductByIdAsync(int id) => _dbContext.Products.FindAsync(id);

        public async ValueTask<bool> CreateProductAsync(Product product)
        {
            try
            {
                await _dbContext.Products.AddAsync(product);

                return true;
            }
            catch
            {
                return true;
            }
        }

        public ValueTask<bool> EditProductAsync(Product product)
        {
            try
            {
                _dbContext.Update(product);

                return ValueTask.FromResult(true);
            }
            catch
            {
                return ValueTask.FromResult(false);
            }
        }

        public async ValueTask<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await GetProductByIdAsync(id);
                return await DeleteProductAsync(product);
            }
            catch
            {
                return false;
            }
        }

        public ValueTask<bool> DeleteProductAsync(Product product)
        {
            try
            {
                _dbContext.Products.Remove(product);

                return ValueTask.FromResult(true);
            }
            catch
            {
                return ValueTask.FromResult(false);
            }
        }

        public async ValueTask<bool> SaveAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
