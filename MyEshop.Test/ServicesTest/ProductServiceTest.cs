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
        private readonly Mock<IImageRepository> _mockImageRepository;
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<ICategoryService> _mockCategorySerivce;
        private readonly IProductService _productService;

        public ProductServiceTest()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockTagRepository = new Mock<ITagRepository>();
            _mockImageRepository = new Mock<IImageRepository>();
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockCategorySerivce = new Mock<ICategoryService>();

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
            Assert.Equal(ErrorMessage.NotExistCategory, isCreateProduct.Errors.FirstOrDefault().Message);
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
            Assert.Equal(ErrorMessage.NotExistTag, isCreateProduct.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task Test_Get_Product_Delete_Reulst_Founded()
        {
            _mockProductRepository.Setup(productRepository =>
                productRepository.GetProductByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(new Product());

            _mockCategorySerivce.Setup(categoryRepository =>
                categoryRepository.GetCategorieChildrenByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(new CategoryViewModel());

            _mockTagRepository.Setup(mockTagRepository =>
                mockTagRepository.GetTagsProductByProductId(It.IsAny<int>()))
                    .Returns(new List<Tag>().AsQueryable());

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.GetImagesProductByProductId(It.IsAny<int>()))
                    .Returns(new List<Image>().AsEnumerable());

            _mockCommentRepository.Setup(commentRepository =>
               commentRepository.GetCommentCountProductByProductId(It.IsAny<int>()))
                   .Returns(0);

            var productDeleteModel = await _productService.GetProductDeleteViewByProductIdAsync(It.IsAny<int>());

            Assert.NotNull(productDeleteModel);
        }

        [Fact]
        public async Task Test_Get_Product_Delete_Reulst_Not_Found()
        {
            _mockProductRepository.Setup(productRepository =>
                productRepository.GetProductByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(null as Product);

            var productDeleteModel = await _productService.GetProductDeleteViewByProductIdAsync(It.IsAny<int>());

            Assert.Null(productDeleteModel);
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Deleted()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImageByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.True(resultProductDelete.IsSuccess);
            Assert.Equal(0, resultProductDelete.Errors.Count());
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_Found()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Product);

            var resultProductDelete = await _productService.DeleteProductAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.True(resultProductDelete.IsNotFound);
            Assert.Equal(0, resultProductDelete.Errors.Count());
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_Delete()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync((It.IsAny<int>())))
                .ReturnsAsync(false);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImageByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Equal(1, resultProductDelete.Errors.Count());
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_Save()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(false);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImageByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Equal(1, resultProductDelete.Errors.Count());
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_DeleteComment()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImageByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultProductDelete = await _productService.DeleteProductAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Equal(1, resultProductDelete.Errors.Count());
        }

        [Fact]
        public async Task Test_Delete_Product_Result_Not_DeleteImage()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Product());

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync((It.IsAny<int>())))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            _mockCommentRepository.Setup(commentRepository => commentRepository.DeleteCommentByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockImageRepository.Setup(imageRepository => imageRepository.DeleteImageByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var resultProductDelete = await _productService.DeleteProductAsync(It.IsAny<int>());

            Assert.NotNull(resultProductDelete);
            Assert.False(resultProductDelete.IsSuccess);
            Assert.Equal(1, resultProductDelete.Errors.Count());
        }
    }
}
