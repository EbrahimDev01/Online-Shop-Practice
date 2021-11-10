using MyEshop.Application.ViewModels.PublicViewModelClass;
using MyEshop.Application.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.Interfaces
{
    public interface ITagService
    {
        public ValueTask<IList<TagForSelect>> GetTagsForSelectAsync();

        public IAsyncEnumerable<TagViewModel> GetAllTagsAsync();

        public ValueTask<ResultMethodService> CreateTagAsync(TagCreateViewModel tagCreateModel);

        public ValueTask<TagDetailsViewModel> GetTagDetailsByTagIdAsync(int tagId);

        public ValueTask<TagEditViewModel> GetTagShapeEditViewModelByTagIdAsync(int tagId);
        public Task<bool> IsExistTagByTagTitleAndTagId(string tagTitle, int tagId);

        public ValueTask<ResultMethodService> EditTagAsync(TagEditViewModel tagEditModel);

        public ValueTask<TagDeleteViewModel> GetTagShapeDeleteViewModelByTagIdAsync(int tagId);

        public ValueTask<ResultMethodService> DeleteTagAsync(int tagId);

        public Task<bool> IsExistTagByTitle(string tagTitle);
    }
}
