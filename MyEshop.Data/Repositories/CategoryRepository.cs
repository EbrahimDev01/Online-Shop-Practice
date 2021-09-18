using Microsoft.EntityFrameworkCore;
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
    public class CategoryRepository : ICategoryRepository
    {

        private readonly MyEshopDBContext _dbContext;

        public CategoryRepository(MyEshopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ValueTask<Category> GetCategoryByIdAsync(int id) => _dbContext.Categories.FindAsync(id);

        public IAsyncEnumerable<Category> GetCategoriesAsync()
            => _dbContext.Categories;

        public Task<bool> IsExistCategoryAsync(int id) => _dbContext.Categories.AnyAsync(c => c.CategoryId == id);

        public IAsyncEnumerable<Category> GetCategoryByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
