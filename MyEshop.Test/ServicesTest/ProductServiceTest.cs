using Microsoft.AspNetCore.Http;
using Moq;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Application.Utilities.File;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Application.ViewModels.Image;
using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.PublicViewModelClass;
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
        private readonly Mock<IFileHandler> _mockFileHandler;
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
            _mockFileHandler = new Mock<IFileHandler>();


            _productService = new ProductService(_mockProductRepository.Object, _mockCategoryRepository.Object,
                    _mockTagRepository.Object, _mockImageRepository.Object,
                    _mockCommentRepository.Object, _mockFileHandler.Object);
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


        #region create product

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Not_Create()
        {
            _mockProductRepository.Setup(mpr => mpr.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(false);

            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
               .Returns(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });


            var resultCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(resultCreateProduct);
            Assert.NotNull(resultCreateProduct);
            Assert.False(resultCreateProduct.IsSuccess);
            Assert.False(resultCreateProduct.IsNotFound);
            Assert.Single(resultCreateProduct.Errors);
            Assert.Contains(resultCreateProduct.Errors,
                error => error.Title == string.Empty && error.Message == ErrorMessage.ExceptionProductCreate("محصول"));
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

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
               .Returns(true);

            var resultCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(resultCreateProduct);
            Assert.NotNull(resultCreateProduct);
            Assert.True(resultCreateProduct.IsSuccess);
            Assert.False(resultCreateProduct.IsNotFound);
            Assert.Empty(resultCreateProduct.Errors);
        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Not_Valid_Type_Files()
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

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
               .Returns(false);

            var resultCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(resultCreateProduct);
            Assert.NotNull(resultCreateProduct);
            Assert.False(resultCreateProduct.IsSuccess);
            Assert.False(resultCreateProduct.IsNotFound);
            Assert.Single(resultCreateProduct.Errors);
            Assert.Contains(resultCreateProduct.Errors,
                error => error.Title == nameof(ProductCreateViewModel.Images) && error.Message == ErrorMessage.ExceptionFileImagesType);
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


            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
               .Returns(true);

            var product = new ProductCreateViewModel();

            var resultCreateProduct = await _productService.CreateProductAsync(product);

            Assert.IsType<ResultMethodService>(resultCreateProduct);
            Assert.NotNull(resultCreateProduct);
            Assert.True(resultCreateProduct.IsSuccess);
            Assert.False(resultCreateProduct.IsNotFound);
            Assert.Equal(0, resultCreateProduct.Errors.Count);
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

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
               .Returns(true);

            var resultCreateProduct = await _productService.CreateProductAsync(new ProductCreateViewModel());

            Assert.IsType<ResultMethodService>(resultCreateProduct);
            Assert.NotNull(resultCreateProduct);
            Assert.False(resultCreateProduct.IsSuccess);
            Assert.Single(resultCreateProduct.Errors);

            Assert.Contains(resultCreateProduct.Errors,
                error => error.Title == nameof(ProductCreateViewModel.CategoryId) &&
                error.Message == ErrorMessage.ExceptionExistCategory);

        }

        [Fact]
        public async Task Test_Create_Product_input_CreateProductViewModel_Result_Not_Exist_Tag()
        {
            _mockCategoryRepository.Setup(mpr => mpr.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
               .Returns(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<Tag>());

            var createProduct = new ProductCreateViewModel
            {
                Tags = new List<TagForSelect>
                {
                    new(){ IsSelected=true},
                    new(){ IsSelected=true},
                }
            };

            var resultCreateProduct = await _productService.CreateProductAsync(createProduct);

            Assert.IsType<ResultMethodService>(resultCreateProduct);
            Assert.NotNull(resultCreateProduct);
            Assert.False(resultCreateProduct.IsSuccess);
            Assert.Single(resultCreateProduct.Errors);

            Assert.Contains(resultCreateProduct.Errors,
                error => error.Title == nameof(ProductCreateViewModel.Tags) &&
                error.Message == ErrorMessage.ExceptionExistTags);
        }

        #endregion

        #region Delete

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

            Assert.Contains(resultProductDelete.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.NotFound("محصول"));
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


            Assert.Contains(resultProductDelete.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionProductDelete);
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

            _mockProductRepository.Setup(productRepository => productRepository.DeleteProductAsync(It.IsAny<Product>()))
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

            Assert.Contains(resultProductDelete.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionSave);
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

            Assert.Contains(resultProductDelete.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionCommentsDelete);
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

            Assert.Contains(resultProductDelete.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionImagesDeletse);
        }

        #endregion

        #region Test GetProductDetailsByIdAsync

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

        #endregion

        #region Test Edit Product

        [Fact]
        public async Task Test_EditProductAsync_Result_Not_Found()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Product);

            var resultProductEdit = await _productService.EditProductAsync(new ProductEditViewModel());

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.True(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
        }

        [Fact]
        public async Task Test_EditProductAsync_Category_Is_Not_Accepted()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultProductEdit = await _productService.EditProductAsync(new ProductEditViewModel());

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == nameof(ProductEditViewModel.CategoryId) && error.Message == ErrorMessage.ExceptionExistCategory);
        }

        [Fact]
        public async Task Test_EditProductAsync_Tags_Is_Not_Accepted()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>());

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var productEdit = new ProductEditViewModel()
            {
                Tags = new List<TagForSelect>() { new() { IsSelected = true } }
            };

            var resultProductEdit = await _productService.EditProductAsync(productEdit);

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == nameof(ProductEditViewModel.Tags) && error.Message == ErrorMessage.ExceptionExistTags);
        }

        [Fact]
        public async Task Test_EditProductAsync_Type_Image_Is_Not_Accepted()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(false);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultProductEdit = await _productService.EditProductAsync(new ProductEditViewModel());

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == nameof(ProductEditViewModel.Images) && error.Message == ErrorMessage.ExceptionFileImagesType);
        }

        [Fact]
        public async Task Test_EditProductAsync_AvailableImages_Is_Not_Accepted()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(false);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var productEdit = new ProductEditViewModel()
            {
                AvailableImages = new List<SelectImageToDelete>() { new() { ImageId = 1, UrlImage = "", IsSelected = true } }
            };
            var resultProductEdit = await _productService.EditProductAsync(productEdit);

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == nameof(ProductEditViewModel.Images) && error.Message == ErrorMessage.ExceptionAvailableImages);


        }

        [Fact]
        public async Task Test_EditProductAsync_Result_Can_Not_Delete_File_Image()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                 imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
                     .ReturnsAsync(false);

            _mockProductRepository.Setup(productRepository => productRepository.EditProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);


            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);


            var productEdit = new ProductEditViewModel()
            {
                AvailableImages = new List<SelectImageToDelete>() { new() { ImageId = 1, UrlImage = "", IsSelected = true } }
            };

            var resultProductEdit = await _productService.EditProductAsync(productEdit);

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == nameof(ProductEditViewModel.Images) && error.Message == ErrorMessage.ExceptionFileImagesDelete);
        }

        [Fact]
        public async Task Test_EditProductAsync_Result_Can_Not_Delete_Tags_Product()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(false);


            var productEdit = new ProductEditViewModel()
            {
                AvailableImages = new List<SelectImageToDelete>() { new() { ImageId = 1, UrlImage = "", IsSelected = true } }
            };

            var resultProductEdit = await _productService.EditProductAsync(productEdit);

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == nameof(ProductEditViewModel.Tags) && error.Message == ErrorMessage.ExceptionTagsDelete);
        }


        [Fact]
        public async Task Test_EditProductAsync_Result_Can_Not_Edit()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
               imageRepository.GetImagesByImageIds(It.IsAny<IEnumerable<int>>()))
                   .Returns(new List<Image>
                   {
                        new(),
                        new(),
                        new(),
                   });


            _mockImageRepository.Setup(imageRepository =>
                 imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
                     .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.EditProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(false);

            var resultProductEdit = await _productService.EditProductAsync(new ProductEditViewModel());

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == string.Empty && error.Message == ErrorMessage.ExceptionProductEdit("محصول"));

        }


        [Fact]
        public async Task Test_EditProductAsync_Result_Can_Not_Save()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });


            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
               imageRepository.GetImagesByImageIds(It.IsAny<IEnumerable<int>>()))
                   .Returns(new List<Image>
                   {
                        new(),
                        new(),
                        new(),
                   });

            _mockImageRepository.Setup(imageRepository =>
                 imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
                     .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.EditProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(false);

            var resultProductEdit = await _productService.EditProductAsync(new ProductEditViewModel());

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.False(resultProductEdit.IsSuccess);
            Assert.Single(resultProductEdit.Errors);
            Assert.Contains(resultProductEdit.Errors,
                error => error.Title == string.Empty && error.Message == ErrorMessage.ExceptionSave);
        }

        [Fact]
        public async Task Test_EditProductAsync_Result_Is_Edited()
        {
            _mockProductRepository.Setup(productRepository => productRepository.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Product());

            _mockCategoryRepository.Setup(categoryRepository => categoryRepository.IsExistCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository =>
                productRepository.DeleteTagsProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(mpr => mpr.GetTagsByIds(It.IsAny<IEnumerable<int>>()))
               .Returns(new List<Tag>
               {
                   new(),
                   new(),
                   new(),
               });

            _mockFileHandler.Setup(fileHandler => fileHandler.IsImage(It.IsAny<IEnumerable<IFormFile>>()))
                .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.IsExistAvailableImages(It.IsAny<IEnumerable<Image>>(), It.IsAny<int>()))
                    .Returns(true);

            _mockImageRepository.Setup(imageRepository =>
                imageRepository.GetImagesByImageIds(It.IsAny<IEnumerable<int>>()))
                    .Returns(new List<Image>
                    {
                        new(),
                        new(),
                        new(),
                    });

            _mockImageRepository.Setup(imageRepository =>
                 imageRepository.DeleteImagesAsync(It.IsAny<IEnumerable<Image>>()))
                     .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.EditProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            _mockProductRepository.Setup(productRepository => productRepository.SaveAsync())
                .ReturnsAsync(true);

            var resultProductEdit = await _productService.EditProductAsync(new ProductEditViewModel());

            Assert.NotNull(resultProductEdit);
            Assert.IsType<ResultMethodService>(resultProductEdit);
            Assert.False(resultProductEdit.IsNotFound);
            Assert.True(resultProductEdit.IsSuccess);
            Assert.Empty(resultProductEdit.Errors);
        }

        #endregion
    }
}
