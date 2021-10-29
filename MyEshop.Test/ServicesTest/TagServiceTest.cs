using Moq;
using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Application.ViewModels.Product;
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
        public async void Test_Create_Tag_Result_Tag_Is_Exist()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(true);

            var resultTagCreate = await _tagService.CreateTagAsync(new TagCreateViewModel());

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
        public async void Test_Create_Tag_Result_Tag_Failed_Add()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(tagRepository => tagRepository.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(false);

            var resultTagCreate = await _tagService.CreateTagAsync(new TagCreateViewModel());

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
        public async void Test_Create_Tag_Result_Tag_Failed_Save()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(tagRepository => tagRepository.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.SaveAsync())
                .ReturnsAsync(false);

            var resultTagCreate = await _tagService.CreateTagAsync(new TagCreateViewModel());

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
        public async void Test_Create_Tag_Result_Tag_Successful()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(tagRepository => tagRepository.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.SaveAsync())
                .ReturnsAsync(true);

            var resultTagCreate = await _tagService.CreateTagAsync(new TagCreateViewModel());

            Assert.NotNull(resultTagCreate);
            Assert.IsType<ResultMethodService>(resultTagCreate);
            Assert.False(resultTagCreate.IsNotFound);
            Assert.True(resultTagCreate.IsSuccess);
            Assert.Empty(resultTagCreate.Errors);
        }

        [Theory]
        [InlineData("apple", true)]
        [InlineData("hp", false)]
        public async void Test_Is_Exist_Tag_With_Name(string tagSample, bool expected)
        {
            var tagsList = new List<string>
            {
                "apple",
                "Microsoft",
                "Samsung",
                "LG"
            };

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTitle(It.IsAny<string>()))
                .ReturnsAsync(tagsList.Any(tag => tag == tagSample));


            bool resultIsExistTagByTitle = await _tagService.IsExistTagByTitle(tagSample);

            Assert.Equal(resultIsExistTagByTitle, expected);
        }

        [Fact]
        public async void Test_GetTagDetailsByTagIdAsync_Result_Not_Found()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagIncludeProductsByTagId(It.IsAny<int>()))
                .ReturnsAsync(null as Tag);

            var resultGetTagDetails = await _tagService.GetTagDetailsByTagIdAsync(1);

            Assert.Null(resultGetTagDetails);
        }

        [Fact]
        public async void Test_GetTagDetailsByTagIdAsync_Input_Method_Is_Zero_Result_Not_Found()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagIncludeProductsByTagId(It.IsAny<int>()))
                 .ReturnsAsync(null as Tag);

            var resultGetTagDetails = await _tagService.GetTagDetailsByTagIdAsync(0);

            Assert.Null(resultGetTagDetails);
        }

        [Fact]
        public async void Test_GetTagDetailsByTagIdAsync_Result_Found()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagIncludeProductsByTagId(It.IsAny<int>()))
                .ReturnsAsync(new Tag() { Products = new List<Product>() { new() } });

            var resultGetTagDetails = await _tagService.GetTagDetailsByTagIdAsync(1);

            Assert.NotNull(resultGetTagDetails);
            Assert.NotNull(resultGetTagDetails.PreviewAdminProducts);
            Assert.IsAssignableFrom<IAsyncEnumerable<PreviewAdminProductViewModel>>(resultGetTagDetails.PreviewAdminProducts);
            Assert.IsType<TagDetailsViewModel>(resultGetTagDetails);
        }

    }
}
