using MyEshop.Application.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Interfaces
{
    public interface ICategoryService
    {
        public IAsyncEnumerable<CategoryViewModel> GetCategoriesChildrenAsync();
        public ValueTask<CategoryViewModel> GetCategorieChildrenByIdAsync(int categoryId);
    }
}
