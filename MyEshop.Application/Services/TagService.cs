using MyEshop.Application.ConstApplication.Names;
using MyEshop.Application.Interfaces;
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

namespace MyEshop.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IImageRepository _imageRepository;

        public TagService(ITagRepository tagRepository, IImageRepository imageRepository)
        {
            _tagRepository = tagRepository;
            _imageRepository = imageRepository;
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

        public async ValueTask<TagDetailsViewModel> GetTagDetailsByTagIdAsync(int tagId)
        {
            if (tagId is 0)
            {
                return null;
            }

            var tag = await _tagRepository.GetTagIncludeProductsByTagId(tagId);

            if (tag is null)
            {
                return null;
            }

            var products = tag.Products.ToAsyncEnumerable()
                .SelectAwait(async product =>
                    new PreviewAdminProductViewModel(
                        product,
                       await _imageRepository.GetFirstImageUrlProductByProductIdAsync(product.ProductId))
                    );

            return new TagDetailsViewModel(tag, products);
        }

        public async ValueTask<TagEditViewModel> GetTagShapeEditViewModelByTagIdAsync(int tagId)
        {
            if (tagId is 0)
            {
                return null;
            }

            var tag = await _tagRepository.GetTagByTagId(tagId);
            if (tag is null)
            {
                return null;
            }

            return new TagEditViewModel(tag);
        }

        public Task<bool> IsExistTagByTagTitleAndTagId(string tagTitle, int tagId)
            => _tagRepository.IsExistTagByTagTitleAndTagId(tagTitle, tagId);

        public async ValueTask<ResultMethodService> EditTagAsync(TagEditViewModel tagEditModel)
        {
            var resultMethod = new ResultMethodService();


            if (tagEditModel.TagId is 0)
            {
                resultMethod.NotFound();

                return resultMethod;
            }


            bool isExistTag = await _tagRepository.IsExistTagByTagTitleAndTagId(tagEditModel.Title, tagEditModel.TagId);
            if (isExistTag)
            {
                resultMethod.AddError(nameof(TagEditViewModel.Title), ErrorMessage.IsExistWithName(DisplayNames.Title));

                return resultMethod;
            }


            var tag = await _tagRepository.GetTagByTagId(tagEditModel.TagId);
            if (tag is null)
            {
                resultMethod.NotFound();

                return resultMethod;
            }

            tag.Title = tagEditModel.Title;

            bool resultEditTag = await _tagRepository.EditTagAsync(tag);
            if (!resultEditTag)
            {
                resultMethod.AddError(string.Empty, ErrorMessage.ExceptionEdit(DisplayNames.Tag));

                return resultMethod;
            }


            bool resultSave = await _tagRepository.SaveAsync();
            if (!resultSave)
            {
                resultMethod.AddError(string.Empty, ErrorMessage.ExceptionSave);

                return resultMethod;
            }


            return resultMethod;
        }
    }
}
