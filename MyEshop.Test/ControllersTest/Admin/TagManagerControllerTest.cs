using Microsoft.AspNetCore.Mvc;
using Moq;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
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
    public class TagManagerControllerTest
    {
        private readonly TagManagerController _tagManagerController;
        private readonly Mock<ITagService> _mockTagService;

        public TagManagerControllerTest()
        {
            _mockTagService = new Mock<ITagService>();

            _tagManagerController = new TagManagerController(_mockTagService.Object);
        }

        [Fact]
        public void Test_Index_Result_Contains_Value()
        {
            _mockTagService.Setup(tagService => tagService.GetAllTagsAsync())
                .Returns(new List<TagViewModel>
                {
                    new()
                }
                .ToAsyncEnumerable());

            var resultProductIndex = _tagManagerController.Index() as ViewResult;

            var resultProductIndexModel = resultProductIndex.Model as IAsyncEnumerable<TagViewModel>;

            Assert.NotNull(resultProductIndex);
            Assert.NotNull(resultProductIndexModel);
            Assert.Single(resultProductIndexModel.ToEnumerable());
        }

        [Fact]
        public void Test_Index_Result_List_Is_Empty()
        {
            _mockTagService.Setup(tagService => tagService.GetAllTagsAsync())
                .Returns(new List<TagViewModel>().ToAsyncEnumerable());

            var resultProductIndex = _tagManagerController.Index() as ViewResult;

            var resultProductIndexModel = resultProductIndex.Model as IAsyncEnumerable<TagViewModel>;

            Assert.NotNull(resultProductIndex);
            Assert.NotNull(resultProductIndexModel);
            Assert.Empty(resultProductIndexModel.ToEnumerable());
        }

        [Fact]
        public void Test_Create_Tag_View_Result_Return_View()
        {
            var resultTagCreate = _tagManagerController.Create() as ViewResult;

            Assert.NotNull(resultTagCreate);
        }

        [Fact]
        public async void Test_Create_Tag_Result_Exception()
        {
            var errorResult = new ErrorResultMethodService("Tag", "ExceptionCreate");
            var resultMethod = new ResultMethodService();

            resultMethod.AddError(errorResult);

            _mockTagService.Setup(tagService => tagService.CreateTagAsync(It.IsAny<TagCreateViewModel>()))
                .ReturnsAsync(resultMethod);

            var resultTagCreate = await _tagManagerController.Create(new TagCreateViewModel()) as ViewResult;

            var resultTagCreateErrors = _tagManagerController.ModelState.Where(y => y.Value.Errors.Count > 0)
                        .Select(x => new ErrorResultMethodService(x.Key, x.Value.Errors.FirstOrDefault().ErrorMessage));

            Assert.NotNull(resultTagCreate);
            Assert.Single(resultTagCreateErrors);
            Assert.False(_tagManagerController.ModelState.IsValid);
            Assert.Contains(resultTagCreateErrors,
                error => error.Title == errorResult.Title &&
                error.Message == errorResult.Message);
        }

        [Fact]
        public async void Test_Create_Tag_Result_Completed()
        {
            var resultMethod = new ResultMethodService();

            _mockTagService.Setup(tagService => tagService.CreateTagAsync(It.IsAny<TagCreateViewModel>()))
                .ReturnsAsync(resultMethod);

            var resultTagCreate = await _tagManagerController.Create(new TagCreateViewModel()) as RedirectToActionResult;

            var resultTagCreateErrors = _tagManagerController.ModelState.Where(y => y.Value.Errors.Count > 0)
                        .Select(x => new ErrorResultMethodService(x.Key, x.Value.Errors.FirstOrDefault().ErrorMessage));

            Assert.NotNull(resultTagCreate);
            Assert.Empty(resultTagCreateErrors);
            Assert.True(_tagManagerController.ModelState.IsValid);
        }

        [Theory]
        [InlineData("apple", false)]
        [InlineData("hp", true)]
        public async void Test_Is_Exist_Tag_With_Name(string tagSample, bool expected)
        {
            var tagsList = new List<string>
            {
                "apple",
                "Microsoft",
                "Samsung",
                "LG"
            };

            _mockTagService.Setup(tagService => tagService.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(tagsList.Any(tag => tag == tagSample));


            var resultIsExistTagByTitle = await _tagManagerController.IsExistTagByTitle(tagSample) as JsonResult;

            Assert.NotNull(resultIsExistTagByTitle);
            Assert.Equal(resultIsExistTagByTitle.Value, expected);
        }

        [Fact]
        public async void Test_Details_Tag_Result_Not_Found()
        {
            _mockTagService.Setup(tagService => tagService.GetTagDetailsByTagIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as TagDetailsViewModel);

            var resultDetailsTag = await _tagManagerController.Details(It.IsAny<int>()) as NotFoundResult;

            Assert.NotNull(resultDetailsTag);
        }

        [Fact]
        public async void Test_Details_Tag_Result_Found()
        {
            _mockTagService.Setup(tagService => tagService.GetTagDetailsByTagIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new TagDetailsViewModel());

            var resultDetailsTag = await _tagManagerController.Details(1) as ViewResult;
            var resultDetailsTagModel = resultDetailsTag.Model;

            Assert.NotNull(resultDetailsTag);
            Assert.NotNull(resultDetailsTagModel);
            Assert.IsType<TagDetailsViewModel>(resultDetailsTagModel);
        }

        [Fact]
        public async void Test_Edit_Tag_Id_Is_Zero_Result_Not_Found()
        {
            var resultEditTag = await _tagManagerController.Edit(0) as NotFoundResult;

            Assert.NotNull(resultEditTag);
        }

        [Fact]
        public async void Test_Edit_Tag_Can_Not_Found_Result_Not_Found()
        {
            _mockTagService.Setup(tagService => tagService.GetTagShapeEditViewModelByTagIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as TagEditViewModel);

            var resultEditTag = await _tagManagerController.Edit(1) as NotFoundResult;

            Assert.NotNull(resultEditTag);
        }


        [Fact]
        public async void Test_Edit_Tag_Result_Found()
        {
            _mockTagService.Setup(tagService => tagService.GetTagShapeEditViewModelByTagIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new TagEditViewModel());

            var resultEditTag = await _tagManagerController.Edit(1) as ViewResult;
            var resultEditTagModel = resultEditTag.Model;

            Assert.NotNull(resultEditTag);
            Assert.IsType<TagEditViewModel>(resultEditTagModel);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async void Test_IsExistTagByTagTitleAndTagId_Action(bool expected)
        {
            _mockTagService.Setup(tagService => tagService.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(expected);


            var resultIsExistTagByTitle = await _tagManagerController.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()) as JsonResult;

            Assert.NotNull(resultIsExistTagByTitle);
            Assert.Equal(resultIsExistTagByTitle.Value, !expected);
        }


    }

}