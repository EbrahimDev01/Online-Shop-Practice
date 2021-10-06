using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Interfaces
{
    public interface IProductRepository
    {
        public IAsyncEnumerable<Product> GetProductsAllAsync();

        public ValueTask<Product> GetProductByIdAsync(int id);

        public ValueTask<bool> CreateProductAsync(Product product);

        public ValueTask<bool> EditProductAsync(Product product);

        public ValueTask<bool> DeleteProductByIdAsync(int id);

        public ValueTask<bool> DeleteProductAsync(Product product);

        public ValueTask<bool> DeleteTagsProductByProductIdAsync(int productId);

        public ValueTask<bool> SaveAsync();
    }
}
