using Microsoft.AspNetCore.Mvc;
using Moq;
using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.Category;
using MyEshop.Application.ViewModels.Image;
using MyEshop.Application.ViewModels.Product;
using MyEshop.Application.ViewModels.PublicViewModelClass;
using MyEshop.Application.ViewModels.Tag;
using MyEshop.Domain.ConstsDomain.Messages;
using MyEshop.Domain.Models;
using MyEshop.Mvc.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.ControllersTest.Admin
{
    public class ProductManagerControllerTest
    {

        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ITagService> _mockTagService;
        private readonly Mock<ICategoryService> _mockCategorytService;
        private readonly ProductManagerController _productController;

        public ProductManagerControllerTest()
        {
            _mockProductService = new Mock<IProductService>();
            _mockTagService = new Mock<ITagService>();
            _mockCategorytService = new Mock<ICategoryService>();
            _productController = new ProductManagerController(_mockProductService.Object, _mockTagService.Object,
                _mockCategorytService.Object);
        }

        [Fact]
        public async Task Test_Index_Get_All_PreviewAdminProduct_Result_3_PreviewAdminProduct()
        {
            _mockProductService.Setup(mps => mps.GetAllPreviewAdminProductsAsync()).Returns(new List<PreviewAdminProductViewModel>
            {
                new PreviewAdminProductViewModel(),
                new PreviewAdminProductViewModel(),
                new PreviewAdminProductViewModel(),
            }.ToAsyncEnumerable());

            var resultProductControllerIndex = _productController.Index() as ViewResult;

            int productsCount = await (resultProductControllerIndex.Model as IAsyncEnumerable<PreviewAdminProductViewModel>).CountAsync();

            Assert.NotNull(resultProductControllerIndex);
            Assert.NotNull(resultProductControllerIndex.Model);
            Assert.Equal(3, productsCount);
        }

        [Fact]
        public void Test_Index_Get_All_PreviewAdminProduct_Result_Not_Found()
        {
            _mockProductService.Setup(mps => mps.GetAllPreviewAdminProductsAsync())
                .Returns((IAsyncEnumerable<PreviewAdminProductViewModel>)null);

            var resultProductControllerIndex = _productController.Index() as ViewResult;

            Assert.NotNull(resultProductControllerIndex);
            Assert.Null(resultProductControllerIndex.Model);
        }

        [Fact]
        public async Task Test_Create_Post_Result_View()
        {
            _mockProductService.Setup(mps => mps.CreateProductAsync(It.IsAny<ProductCreateViewModel>()))
                .ReturnsAsync(new ResultMethodService());

            var productControllerCreate = await _productController.Create(new ProductCreateViewModel()) as RedirectToActionResult;

            Assert.NotNull(productControllerCreate);
            Assert.Equal("Index", productControllerCreate.ActionName);
        }

        [Fact]
        public async Task Test_Create_Result_Model_State_Not_Valid()
        {
            _productController.ModelState.AddModelError("", "");

            var productControllerCreate = await _productController.Create(new ProductCreateViewModel()) as ViewResult;

            Assert.NotNull(productControllerCreate);
            Assert.NotNull(productControllerCreate.Model);
            Assert.False(_productController.ModelState.IsValid);
        }

        [Fact]
        public async Task Test_Create_Result_Error_In_Layer_Service()
        {

            var resultMethodService = new ResultMethodService();
            resultMethodService.AddError("", "");

            _mockProductService.Setup(mps => mps.CreateProductAsync(It.IsAny<ProductCreateViewModel>()))
                .ReturnsAsync(resultMethodService);

            var productControllerCreate = await _productController.Create(new ProductCreateViewModel()) as ViewResult;

            Assert.NotNull(productControllerCreate);
            Assert.NotNull(productControllerCreate.Model);
            Assert.False(_productController.ModelState.IsValid);
        }

        [Fact]
        public async Task Test_Create_Rresult_View_Result()
        {
            var categoriesList = new List<CategoryViewModel>
            {
                new(),
                new(),
                new()
            }.ToAsyncEnumerable();
            var tagsList = new List<TagForSelect>
            {
                new(),
                new(),
                new()
            };

            _mockCategorytService.Setup(mcs => mcs.GetCategoriesChildrenAsync())
                .Returns(categoriesList);

            _mockTagService.Setup(mcs => mcs.GetTagsForSelectAsync())
                .ReturnsAsync(tagsList);

            var resultProductCreate = (await _productController.Create()) as ViewResult;

            var resultProductCreateModel = resultProductCreate.Model as ProductCreateViewModel;

            Assert.NotNull(resultProductCreate);
            Assert.NotNull(resultProductCreateModel);
            Assert.NotNull(resultProductCreateModel.Categories);
            Assert.NotNull(resultProductCreateModel.Tags);
        }

        [Fact]
        public async Task Test_Delete_View_Result()
        {
            _mockProductService.Setup(mockProductService => mockProductService.GetProductDetailsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDetailsViewModel());


            var resultProductDelete = await _productController.Delete(It.IsAny<int>()) as ViewResult;

            Assert.NotNull(resultProductDelete);
            Assert.NotNull(resultProductDelete.Model);
            Assert.IsType<ProductDetailsViewModel>(resultProductDelete.Model);
        }

        [Fact]
        public async Task Test_Delete_Result_Not_Found()
        {
            _mockProductService.Setup(mockProductService => mockProductService.GetProductDetailsByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(null as ProductDetailsViewModel);

            var resultProductDelete = await _productController.Delete(It.IsAny<int>()) as NotFoundResult;

            Assert.NotNull(resultProductDelete);
        }

        [Fact]
        public async Task Test_DeleteConfirm_Result_Deleted()
        {
            _mockProductService.Setup(productService => productService.DeleteProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ResultMethodService());

            var resultProductDeleteConfirm = await _productController.DeleteConfirm(It.IsAny<int>()) as RedirectToActionResult;

            Assert.NotNull(resultProductDeleteConfirm);
            Assert.Equal(nameof(ProductManagerController.Index), resultProductDeleteConfirm.ActionName);
        }

        [Fact]
        public async Task Test_DeleteConfirm_Result_Not_Found()
        {
            var resultMethod = new ResultMethodService();
            resultMethod.NotFound();

            _mockProductService.Setup(productService => productService.DeleteProductByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(resultMethod);

            var resultProductDeleteConfirm = await _productController.DeleteConfirm(It.IsAny<int>()) as NotFoundResult;

            Assert.NotNull(resultProductDeleteConfirm);
        }

        [Fact]
        public async Task Test_Details_Result_Founded()
        {
            _mockProductService.Setup(productService => productService.GetProductDetailsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDetailsViewModel());

            var resultProductDetails = await _productController.Details(It.IsAny<int>()) as ViewResult;

            Assert.NotNull(resultProductDetails);
            Assert.NotNull(resultProductDetails.Model);
            Assert.IsType<ProductDetailsViewModel>(resultProductDetails.Model);
        }

        [Fact]
        public async Task Test_Details_Result_Not_Founded()
        {
            _mockProductService.Setup(productService => productService.GetProductDetailsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as ProductDetailsViewModel);

            var resultProductDetails = await _productController.Details(It.IsAny<int>()) as NotFoundResult;

            Assert.NotNull(resultProductDetails);
        }

        [Fact]
        public async Task Test_Edit_Product_Rresult_View_Result()
        {
            _mockProductService.Setup(productService => productService.GetProductEditDetailsByProductIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new ProductEditViewModel()
               {
                   Categories = new List<CategoryViewModel>().ToAsyncEnumerable(),
                   Tags = new List<TagForSelect>(),
                   AvailableImages = new List<SelectImageToDelete>()
               });


            var resultProductEdit = (await _productController.Edit(It.IsAny<int>())) as ViewResult;

            var resultProductEditModel = resultProductEdit.Model as ProductEditViewModel;

            Assert.NotNull(resultProductEdit);
            Assert.NotNull(resultProductEditModel);
            Assert.NotNull(resultProductEditModel.Categories);
            Assert.NotNull(resultProductEditModel.Tags);
            Assert.NotNull(resultProductEditModel.AvailableImages);
        }

        [Fact]
        public async Task Test_Edit_Product_Rresult_Not_Found()
        {
            _mockProductService.Setup(productService => productService.GetProductEditDetailsByProductIdAsync(It.IsAny<int>()))
               .ReturnsAsync(null as ProductEditViewModel);

            var resultProductEdit = (await _productController.Edit(It.IsAny<int>())) as NotFoundResult;

            Assert.NotNull(resultProductEdit);
        }

        [Fact]
        public async Task Test_Edti_Post_Product_Result_Not_Found()
        {
            _mockProductService.Setup(productService => productService.EditProductAsync(It.IsAny<ProductEditViewModel>()))
                .ReturnsAsync(new ResultMethodService(false, true));

            var resultProductEdit = await _productController.Edit(new ProductEditViewModel(), It.IsAny<int>()) as NotFoundResult;

            Assert.NotNull(resultProductEdit);
        }

        [Fact]
        public async Task Test_Edti_Post_Product_Result_Model_State_Not_Valid()
        {
            _productController.ModelState.AddModelError("", "");

            _mockProductService.Setup(productService => productService.EditProductAsync(It.IsAny<ProductEditViewModel>()))
                .ReturnsAsync(new ResultMethodService());

            _mockProductService.Setup(productService => productService.GetProductEditDetailsByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductEditViewModel());

            var resultProductEdit = await _productController.Edit(new ProductEditViewModel(), It.IsAny<int>()) as ViewResult;

            Assert.NotNull(resultProductEdit);
            _productController.ModelState.Clear();
        }

        [Fact]
        public async Task Test_Edti_Post_Product_Result_Not_Saved()
        {
            var resultMethodService = new ResultMethodService();
            resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionSave);

            _mockProductService.Setup(productService => productService.EditProductAsync(It.IsAny<ProductEditViewModel>()))
                .ReturnsAsync(resultMethodService);

            _mockProductService.Setup(productService => productService.GetProductEditDetailsByProductIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductEditViewModel());

            var resultProductEdit = await _productController.Edit(new ProductEditViewModel(), It.IsAny<int>()) as ViewResult;



            Assert.NotNull(resultProductEdit);
            Assert.False(_productController.ModelState.IsValid);
            Assert.Equal(1, _productController.ModelState.ErrorCount);
        }


        [Fact]
        public async Task Test_Edti_Post_Product_Result_Edited()
        {
            _mockProductService.Setup(productService => productService.EditProductAsync(It.IsAny<ProductEditViewModel>()))
                .ReturnsAsync(new ResultMethodService());

            var resultProductEdit = await _productController.Edit(new ProductEditViewModel(), It.IsAny<int>()) as RedirectToActionResult;

            Assert.NotNull(resultProductEdit);
            Assert.Equal(nameof(ProductManagerController.Index), resultProductEdit.ActionName);
        }
    }
}
