using Microsoft.AspNetCore.Mvc;
using Moq;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.PublicViewModelClass;
using MyEshop.Application.ViewModels.Tag;
using MyEshop.Domain.ConstsDomain.Messages;
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
        public void Test_Create_View_Result_Return_View()
        {
            var resultTagCreate = _tagManagerController.Create() as ViewResult;

            Assert.NotNull(resultTagCreate);
        }

        [Fact]
        public void Test_Create_Result_Model_State_Not_Valid()
        {
            _tagManagerController.ModelState.AddModelError("", "");
            var resultTagCreate = _tagManagerController.Create(new TagCreateViewModel()) as ViewResult;

            Assert.NotNull(resultTagCreate);
        }

        [Fact]
        public void Test_Create_Result_Is_Not_Exist_Tag()
        {
            _mockTagService.Setup(tagService => tagService.IsExistTagByTitle(It.IsAny<string>()))
                .ResultAsync(false);

            var resultTagCreate = _tagManagerController.Create(new TagCreateViewModel()) as ViewResult;

            var resultTagCreateErrors = _tagManagerController.ModelState.Where(y => y.Value.Errors.Count > 0)
                        .Select(x => new ErrorResultMethodService(x.Key, x.Value.Errors.FirstOrDefault().ErrorMessage));

            Assert.NotNull(resultTagCreate);
            Assert.Equal(1, _tagManagerController.ModelState.ErrorCount);
            Assert.False(_tagManagerController.ModelState.IsValid);
            Assert.Contains(resultTagCreateErrors,
                error => error.Title == nameof(TagCreateViewModel.Title) &&
                error.Message == ErrorMessage.IsExistWithName(DisplayNames.Tag));
        }
    }
}