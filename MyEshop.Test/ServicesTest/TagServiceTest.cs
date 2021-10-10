using Moq;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Application.ViewModels.Tag;
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
        public async Task Test_GetAllTags_Result_List_Empty()
        {
            var tagsList = new List<Tag>().ToAsyncEnumerable();

            _mockTagRepository.Setup(tagRepository => tagRepository.GetAllTagsAsync()).Returns(tagsList);

            var tags = await _tagService.GetAllTagsAsync();

            Assert.NotNull(tags);
            Assert.IsAssignableFrom<IAsyncEnumerable<TagViewModel>>(tags);
            Assert.Empty(tags);
        }
    }
}
