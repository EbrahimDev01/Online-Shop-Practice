using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async ValueTask<CategoryViewModel> GetCategorieChildrenByIdAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null) return null;

            var categoryParent = await _categoryRepository.GetCategoryByIdAsync(category.CategoryParentId.Value);

            return new(category, categoryParent);
        }

        public IAsyncEnumerable<CategoryViewModel> GetCategoriesChildrenAsync()
        => _categoryRepository.GetCategoriesAsync()
                .Where(c => c.CategoryParentId != null)
                .SelectAwait(async c => new CategoryViewModel(c, (await _categoryRepository.GetCategoryByIdAsync(c.CategoryParentId.Value))?.Title));

    }
}
