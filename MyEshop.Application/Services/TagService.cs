using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
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

namespace MyEshop.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IAsyncEnumerable<TagViewModel> GetAllTagsAsync()
            => _tagRepository.GetTagsAsync().Select(tag => new TagViewModel(tag));

        public async ValueTask<IList<TagForSelect>> GetTagsForSelectAsync()
            => await _tagRepository.GetTagsAsync().Select(t => new TagForSelect(t)).ToListAsync();

        public async ValueTask<ResultMethodService> CreateTagAsync(TagCreateViewModel tagCreateModel)
        {
            var resultMethodService = new ResultMethodService();

            bool isExistTag = await _tagRepository.IsExistTagByTitle(tagCreateModel.Title);

            if (isExistTag)
            {
                resultMethodService.AddError(nameof(TagCreateViewModel.Title), ErrorMessage.IsExistWithName(DisplayNames.Tag));

                return resultMethodService;
            }


            bool isCreate = await _tagRepository.CreateTagAsync(new Tag(tagCreateModel.Title));

            if (!isCreate)
            {
                resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionCreate(DisplayNames.Tag));

                return resultMethodService;
            }


            bool isSave = await _tagRepository.SaveAsync();

            if (!isSave)
            {
                resultMethodService.AddError(string.Empty, ErrorMessage.ExceptionSave);
            }

            return resultMethodService;
        }

        public Task<bool> IsExistTagByTitle(string tagTitle)
            => _tagRepository.IsExistTagByTitle(tagTitle);
    }
}
