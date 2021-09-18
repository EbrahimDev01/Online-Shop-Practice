using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<bool> IsExistCategoryAsync(int id);
        public IAsyncEnumerable<Category> GetCategoriesAsync();
        public ValueTask<Category> GetCategoryByIdAsync(int id);
        
    }
}
