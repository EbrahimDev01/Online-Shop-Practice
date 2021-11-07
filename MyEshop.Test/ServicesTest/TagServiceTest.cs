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
        private readonly Mock<IImageRepository> _mockIImageRepository;
        private readonly ITagService _tagService;

        public TagServiceTest()
        {
            _mockTagRepository = new Mock<ITagRepository>();
            _mockIImageRepository = new Mock<IImageRepository>();

            _tagService = new TagService(_mockTagRepository.Object, _mockIImageRepository.Object);
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

        [Fact]
        public async void Test_GetTagShapeEditViewModelByTagIdAsync_Id_Is_Zero_Result_Not_Found_Tag()
        {
            var resultTagShapeEdit = await _tagService.GetTagShapeEditViewModelByTagIdAsync(0);


            Assert.Null(resultTagShapeEdit);
        }

        [Fact]
        public async void Test_GetTagShapeEditViewModelByTagIdAsync_Id_Not_Available_Result_Not_Found_Tag()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(null as Tag);

            var resultTagShapeEdit = await _tagService.GetTagShapeEditViewModelByTagIdAsync(1);

            Assert.Null(resultTagShapeEdit);
        }

        [Fact]
        public async void Test_GetTagShapeEditViewModelByTagIdAsync_Result_Found_Tag()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(new Tag());

            var resultTagShapeEdit = await _tagService.GetTagShapeEditViewModelByTagIdAsync(1);

            Assert.NotNull(resultTagShapeEdit);
            Assert.IsType<TagEditViewModel>(resultTagShapeEdit);
        }

        [Theory]
        [InlineData("Apple", 1, false)]
        [InlineData("hp", 2, false)]
        [InlineData("LG", 4, true)]
        public async void Test_IsExistTagByTagTitleAndTagId_Method_Tag_Service(string tagSample, int id, bool expected)
        {
            var tagsList = new List<Tag>
            {
                new Tag("Apple")
                {
                    TagId=1
                },
                new Tag("LG")
                {
                    TagId=2,
                },
                new Tag("Samsung")
                {
                    TagId=3,
                },
                new Tag("Microsoft"){
                    TagId=4,
                }
            };

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(tagsList.Any(tag => tag.Title == tagSample && tag.TagId != id));


            var resultIsExistTagByTitle = await _tagService.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>());

            Assert.Equal(resultIsExistTagByTitle, expected);
        }

        [Fact]
        public async void Test_EditTagAsync_Tag_Id_Is_Zero_Result_Not_Found_Is_True()
        {
            var resultEditTag = await _tagService.EditTagAsync(new TagEditViewModel { TagId = 0 });

            Assert.NotNull(resultEditTag);
            Assert.IsType<ResultMethodService>(resultEditTag);
            Assert.True(resultEditTag.IsNotFound);
            Assert.False(resultEditTag.IsSuccess);
        }

        [Fact]
        public async void Test_EditTagAsync_Result_Is_Exist_Title()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(new Tag());

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var resultEditTag = await _tagService.EditTagAsync(new TagEditViewModel() { TagId = 1 });

            Assert.NotNull(resultEditTag);
            Assert.IsType<ResultMethodService>(resultEditTag);
            Assert.False(resultEditTag.IsNotFound);
            Assert.False(resultEditTag.IsSuccess);
            Assert.Single(resultEditTag.Errors);
            Assert.Contains(resultEditTag.Errors,
                error =>
                error.Title == nameof(TagEditViewModel.Title) && error.Message == ErrorMessage.IsExistWithName(DisplayNames.Title));
        }


        [Fact]
        public async void Test_EditTagAsync_Not_Found_Tag_By_Id_Result_Not_Found_Is_True()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(null as Tag);

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            var resultEditTag = await _tagService.EditTagAsync(new TagEditViewModel());

            Assert.NotNull(resultEditTag);
            Assert.IsType<ResultMethodService>(resultEditTag);
            Assert.True(resultEditTag.IsNotFound);
            Assert.False(resultEditTag.IsSuccess);
        }

        [Fact]
        public async void Test_EditTagAsync_Edit_Exception_Result_Single_Error()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(new Tag());

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(tagRepository => tagRepository.EditTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(false);


            var resultEditTag = await _tagService.EditTagAsync(new TagEditViewModel(){ TagId = 1 });

            Assert.NotNull(resultEditTag);
            Assert.IsType<ResultMethodService>(resultEditTag);
            Assert.False(resultEditTag.IsNotFound);
            Assert.False(resultEditTag.IsSuccess);
            Assert.Single(resultEditTag.Errors);
            Assert.Contains(resultEditTag.Errors,
                error =>
                error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionEdit(DisplayNames.Tag));
        }

        [Fact]
        public async void Test_EditTagAsync_Save_Exception_Result_Single_Error()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(new Tag());

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(tagRepository => tagRepository.EditTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.SaveAsync())
                .ReturnsAsync(false);

            var resultEditTag = await _tagService.EditTagAsync(new TagEditViewModel() { TagId = 1 });

            Assert.NotNull(resultEditTag);
            Assert.IsType<ResultMethodService>(resultEditTag);
            Assert.False(resultEditTag.IsNotFound);
            Assert.False(resultEditTag.IsSuccess);
            Assert.Single(resultEditTag.Errors);
            Assert.Contains(resultEditTag.Errors,
                error =>
                error.Title == string.Empty &&
                error.Message == ErrorMessage.ExceptionSave);
        }

        [Fact]
        public async void Test_EditTagAsync_Exception_Result_Complete_Edit_Tag()
        {
            _mockTagRepository.Setup(tagRepository => tagRepository.GetTagByTagId(It.IsAny<int>()))
                .ReturnsAsync(new Tag());

            _mockTagRepository.Setup(tagRepository => tagRepository.IsExistTagByTagTitleAndTagId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            _mockTagRepository.Setup(tagRepository => tagRepository.EditTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            _mockTagRepository.Setup(tagRepository => tagRepository.SaveAsync())
                .ReturnsAsync(true);

            var resultEditTag = await _tagService.EditTagAsync(new TagEditViewModel() { TagId = 1 });

            Assert.NotNull(resultEditTag);
            Assert.IsType<ResultMethodService>(resultEditTag);
            Assert.False(resultEditTag.IsNotFound);
            Assert.True(resultEditTag.IsSuccess);
            Assert.Empty(resultEditTag.Errors);
        }
    }
}
