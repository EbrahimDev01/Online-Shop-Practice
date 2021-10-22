using Moq;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Application.ViewModels.PublicViewModelClass;
using MyEshop.Application.ViewModels.Tag;
using MyEshop.Domain.ConstsDomain.Messages;
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
    public class TagServiceTest
    {
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly ITagService _tagService;

        public TagServiceTest()
        {
            _mockTagRepository = new Mock<ITagRepository>();

            _tagService = new TagService(_mockTagRepository.Object);
        }

        [Fact]
        public async Task Test_Get_Tags_Result_3_Result()
        {
            var tagsList = new List<Tag> {
            new Tag(),
            new Tag(),
            new Tag(),
            }.ToAsyncEnumerable();

            _mockTagRepository.Setup(mtr => mtr.GetTagsAsync()).Returns(tagsList);

            var tags = await _tagService.GetTagsForSelectAsync();

            Assert.NotNull(tags);
            Assert.IsAssignableFrom<IList<TagForSelect>>(tags);
        }

        [Fact]
        public void Test_GetAllTags_Result_List_Empty()
        {
            var tagsList = new List<Tag>().ToAsyncEnumerable();

            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagsAsync()).Returns(tagsList);

            var tags = _tagService.GetAllTagsAsync().ToEnumerable();

            Assert.NotNull(tags);
            Assert.IsAssignableFrom<IEnumerable<TagViewModel>>(tags);
            Assert.Empty(tags);
        }

        [Fact]
        public void Test_GetAllTags_Result_Single_Item_List()
        {
            var tagsList = new List<Tag> { new() }.ToAsyncEnumerable();

            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagsAsync()).Returns(tagsList);

            var tags = _tagService.GetAllTagsAsync().ToEnumerable();

            Assert.NotNull(tags);
            Assert.IsAssignableFrom<IEnumerable<TagViewModel>>(tags);
            Assert.Single(tags);
        }

        [Fact]
        public void Test_Create_Tag_Result_Tag_Is_Exist()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(false);

            var resultTagCreate = _tagService.CreateTagAsync(new TagCreateViewModel());

            Assert.NotNull(resultTagCreate);
            Assert.IsType<ResultMethodService>(resultTagCreate);
            Assert.False(resultTagCreate.IsNotFound);
            Assert.False(resultTagCreate.IsSuccess);
            Assert.Single(resultTagCreate.Errors);

            Assert.Contains(resultTagCreate.Errors,
                error => error.Title == nameof(TagCreateViewModel.Title) &&
                error.Message == ErrorMessage.IsExistWithName(DisplayNames.Tag));
        }

        [Fact]
        public void Test_Create_Tag_Result_Tag_Failed_Add()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(false);

            var resultTagCreate = _tagService.CreateTagAsync(new TagCreateViewModel());

            Assert.NotNull(resultTagCreate);
            Assert.IsType<ResultMethodService>(resultTagCreate);
            Assert.False(resultTagCreate.IsNotFound);
            Assert.False(resultTagCreate.IsSuccess);
            Assert.Single(resultTagCreate.Errors);

            Assert.Contains(resultTagCreate.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionCreate(DisplayNames.Tag));
        }

        [Fact]
        public void Test_Create_Tag_Result_Tag_Failed_Save()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.SaveAsync())
                .ReturnsAsync(false);

            var resultTagCreate = _tagService.CreateTagAsync(new TagCreateViewModel());

            Assert.NotNull(resultTagCreate);
            Assert.IsType<ResultMethodService>(resultTagCreate);
            Assert.False(resultTagCreate.IsNotFound);
            Assert.False(resultTagCreate.IsSuccess);
            Assert.Single(resultTagCreate.Errors);

            Assert.Contains(resultTagCreate.Errors,
                error => error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionSave);
        }

        [Fact]
        public void Test_Create_Tag_Result_Tag_Successful()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.SaveAsync())
                .ReturnsAsync(true);

            var resultTagCreate = _tagService.CreateTagAsync(new TagCreateViewModel());

            Assert.NotNull(resultTagCreate);
            Assert.IsType<ResultMethodService>(resultTagCreate);
            Assert.False(resultTagCreate.IsNotFound);
            Assert.True(resultTagCreate.IsSuccess);
            Assert.Empty(resultTagCreate.Errors);
        }
    }
}
