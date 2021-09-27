using Microsoft.AspNetCore.Http;
using Moq;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.Public;
using MyEshop.Application.ViewModels.Tag;
using MyEshop.Data.Repositories;
using MyEshop.Domain.ConstsDomain.Messages;
using MyEshop.Domain.Interfaces;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.ServicesTest
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly Mock<ITagService> _mockTagService;
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly IProductService _productService;

        public ProductServiceTest()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockTagRepository = new Mock<ITagRepository>();
            _mockImageRepository = new Mock<IImageRepository>();
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockTagService = new Mock<ITagService>();


            _productService = new ProductService(_mockProductRepository.Object, _mockCategoryRepository.Object,
                    _mockTagRepository.Object, _mockImageRepository.Object,
                    _mockCommentRepository.Object);
        }

        [Fact]
        public async Task Test_Get_All_With_Type_PreviewAdminProducts_Result_3_Products()
        {
            var productList = new List<Product>
            {
                new Product(),
                new Product(),
                new Product()
            }.ToAsyncEnumerable();

            _mockProductRepository.Setup(mpr => mpr.GetProductsAllAsync())
                .Returns(productList);

            var products = _productService.GetAllPreviewAdminProductsAsync();

            int productsCount = await products.CountAsync();

            Assert.NotNull(products);
            Assert.Equal(3, productsCount);
            Assert.IsAssignableFrom<IAsyncEnumerable<PreviewAdminProductViewModel>>(products);
        }

        [Fact]
        public void Test_Get_All_With_Type_PreviewAdminProducts_Result_Not_Found()
        {
            _mockProductRepository.Setup(mpr => mpr.GetProductsAllAsync())
                .Returns((IAsyncEnumerable<Product>)null);

            var products = _productService.GetAllPreviewAdminProductsAsync();

            Assert.Null(products);
        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Not_Create()
        {
            _mockProductRepository.Setup(mpr => mpr.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(false);

            _mockProductRepository.Setup(mpr => mpr.SaveAsync())
                .ReturnsAsync(false);

            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });


            var isCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(isCreateProduct);
            Assert.NotNull(isCreateProduct);
            Assert.False(isCreateProduct.IsSuccess);
            Assert.False(isCreateProduct.IsNotFound);
            Assert.Single(isCreateProduct.Errors);
        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Is_Create()
        {
            _mockProductRepository.Setup(mpr => mpr.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(mpr => mpr.SaveAsync())
                .ReturnsAsync(true);

            _mockProductRepository.Setup(mpr => mpr.SaveAsync())
               .ReturnsAsync(true);

            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });


            var isCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(isCreateProduct);
            Assert.NotNull(isCreateProduct);
            Assert.True(isCreateProduct.IsSuccess);
            Assert.False(isCreateProduct.IsNotFound);
            Assert.Equal(0, isCreateProduct.Errors.Count);
        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Is_Create_And_Save_Image()
        {
            _mockProductRepository.Setup(mpr => mpr.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(mpr => mpr.SaveAsync())
                .ReturnsAsync(true);

            _mockProductRepository.Setup(mpr => mpr.SaveAsync())
               .ReturnsAsync(true);

            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });


            var product = new ProductCreateViewModel();

            var isCreateProduct = await _productService.CreateProductAsync(product);

            Assert.IsType<ResultMethodService>(isCreateProduct);
            Assert.NotNull(isCreateProduct);
            Assert.True(isCreateProduct.IsSuccess);
            Assert.False(isCreateProduct.IsNotFound);
            Assert.Equal(0, isCreateProduct.Errors.Count);
        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Not_Exist_Category()
        {
            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });


            var isCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(isCreateProduct);
            Assert.NotNull(isCreateProduct);
            Assert.False(isCreateProduct.IsSuccess);
            Assert.Single(isCreateProduct.Errors);
            Assert.Equal(nameof(ProductCreateViewModel.CategoryId), isCreateProduct.Errors.FirstOrDefault().Title);
            Assert.Equal(ErrorMessage.ExceptionExistCategory, isCreateProduct.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Not_Exist_Tag()
        {
            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<Tag>());

            var createProduct = new ProductCreateViewModel
            {
                Tags = new List<TagForSelect>
                {
                    new(){ IsSelected=true},
                    new(){ IsSelected=true},
                }
            };

            var isCreateProduct = await _productService.CreateProductAsync(createProduct);

            Assert.IsType<ResultMethodService>(isCreateProduct);
            Assert.NotNull(isCreateProduct);
            Assert.False(isCreateProduct.IsSuccess);
            Assert.Single(isCreateProduct.Errors);
            Assert.Equal(nameof(ProductCreateViewModel.Tags), isCreateProduct.Errors.FirstOrDefault().Title);
            Assert.Equal(ErrorMessage.ExceptionExistTag, isCreateProduct.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Deleted()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync((It.IsAny<Product>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentsByProductId(It.IsAny<int>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
                .ReturnsAsync(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Returns(new List<Image>());

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
               .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.True(resultProductDelete.IsSuccess);
            Assert.Empty(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_Found()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Product);

            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.True(resultProductDelete.IsNotFound);
            Assert.Single(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_Delete()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductByIdAsync((It.IsAny<int>())))
                .ReturnsAsync(false);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentsByProductId(It.IsAny<int>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Returns(new List<Image>());

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
               .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Single(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_Save()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductByIdAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(false);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentsByProductId(It.IsAny<int>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Returns(new List<Image>());

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
               .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Single(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_DeleteComment()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductByIdAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentsByProductId(It.IsAny<int>()))
                .Returns(false);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Returns(new List<Image>());

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
               .ReturnsAsync(true);


            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Single(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_DeleteImage()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductByIdAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentsByProductId(It.IsAny<int>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Returns(new List<Image>());

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
               .ReturnsAsync(false);


            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Single(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductByIdAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentsByProductId(It.IsAny<int>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Throws(new Exception());

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
               .ReturnsAsync(false);


            var resultProductDelete = await _productService.DeleteProductByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Single(resultProductDelete.Errors);
        }

        [Fact]
        public async Task Test_GetProductDetailsByIdAsync_Result_Found()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            var product = await _productService.GetProductDetailsByIdAsync(It.IsAny<int>());

            Assert.NotNull(product);
            Assert.IsType<ProductDetailsViewModel>(product);
        }

        [Fact]
        public async Task Test_GetProductDetailsByIdAsync_Result_Not_Found()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Product);

            var product = await _productService.GetProductDetailsByIdAsync(It.IsAny<int>());

            Assert.Null(product);
        }

        [Fact]
        public async Task Test_GetProductEditDetailsByProductIdAsync_Result_Found()
        {
            var categoriesList = new List<Category>
            {
                new(){CategoryParentId = 1},
                new(){CategoryParentId = 2},
                new(){CategoryParentId = 3}
            }.ToAsyncEnumerable();
            var tagsProduct = new List<Tag>
            {
                new(),
                new(),
                new()
            }.AsQueryable();
            var tagsList = tagsProduct.ToAsyncEnumerable();
            var imageList = new List<Image>
            {
                new(),
                new(),
                new()
            }.AsEnumerable();

            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryService => categoryService.GetCategoriesAsync())
                .Returns(categoriesList);


            _mockCategoryRepository.Setup(categoryService => categoryService.GetCategoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Category(""));

            _mockTagRepository.Setup(tagService => tagService.GetTagsAsync())
                .Returns(tagsList);

            _mockTagRepository.Setup(tagService => tagService.GetTagsProductByProductId(It.IsAny<int>()))
                .Returns(tagsProduct);

            _mockImageRepository.Setup(imageRepository => imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
               .Returns(imageList);

            var product = await _productService.GetProductEditDetailsByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(product);
            Assert.IsType<ProductEditViewModel>(product);
}

        [Fact]
        public async Task Test_GetProductEditDetailsByProductIdAsync_Result_Not_Found()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Product);

            var product = await _productService.GetProductEditDetailsByProductIdAsync(It.IsAny<int>());

            Assert.Null(product);
        }
    }
}
