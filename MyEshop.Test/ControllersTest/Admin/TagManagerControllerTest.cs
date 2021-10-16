﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using MyEshop.Application.Interfaces;
using MyEshop.Application.ViewModels.Tag;
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
    }
}