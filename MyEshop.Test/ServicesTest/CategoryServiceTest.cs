using Moq;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Domain.Interfaces;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.ServicesTest
{
    public class CategoryServiceTest
    {
        private readonly ICategoryService _categoryService;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;

        public CategoryServiceTest()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task Test_Get_Categories_Rresult_3_Tag()
        {
            var categoriesList = new List<Category>
            {
                new Category { CategoryParentId = 1, CategoryParent = new Category(){ CategoryParentId=1} },
                new Category { CategoryParentId = 2, CategoryParent = new Category{ CategoryParentId=2} },
                new Category { CategoryParentId = 2, CategoryParent = new Category{ CategoryParentId=2} },
            }.ToAsyncEnumerable();

            _mockCategoryRepository.Setup(mcr => mcr.GetCategoriesAsync())
                .Returns(categoriesList);

            var categories = _categoryService.GetCategoriesChildrenAsync();

            int categoriesCount = await categories.CountAsync();

            Assert.NotNull(categories);
            Assert.IsAssignableFrom<IAsyncEnumerable<CategoryViewModel>>(categories);
            Assert.Equal(3, categoriesCount);
        }

        [Fact]
        public async Task Test_GetCategorieChildrenByProductIdAsync_Result_Found()
        {
            var categoryPrent = new Category()
            {
                CategoryId = 1,
                Title = ""
            };

            var category = new Category()
            {
                CategoryId = 1,
                Title = "",
                CategoryParent = categoryPrent,
                CategoryParentId = categoryPrent.CategoryId
            };
            categoryPrent.CategoriesChildren.Add(category);

            _mockCategoryRepository.Setup(mockCategoryRepository =>
                mockCategoryRepository.GetCategoryByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(category);

            var resultCategory = await _categoryService.GetCategorieChildrenByIdAsync(It.IsAny<int>());

            Assert.NotNull(resultCategory);
            Assert.IsType<CategoryViewModel>(resultCategory);
        }

        [Fact]
        public async Task Test_GetCategorieChildrenByProductIdAsync_Result_Not_Found()
        {
            _mockCategoryRepository.Setup(mockCategoryRepository =>
                mockCategoryRepository.GetCategoryByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(null as Category);

            var category = await _categoryService.GetCategorieChildrenByIdAsync(It.IsAny<int>());

            Assert.Null(category);
        }
    }
}
