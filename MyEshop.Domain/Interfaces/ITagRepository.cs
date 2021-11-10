using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Interfaces
{
    public interface ITagRepository
    {
        public IEnumerable<Tag> GetTagsByIds(IEnumerable<int> ids);

        public IAsyncEnumerable<Tag> GetTagsAsync();

        public IQueryable<Tag> GetTagsProductByProductId(int ProductId);

        public Task<bool> IsExistTagByTitle(string title);

        public Task<bool> CreateTagAsync(Tag tag);

        public Task<bool> IsExistTagByTagTitleAndTagId(string tagTitle, int tagId);

        public ValueTask<Tag> GetTagByTagId(int tagId);

        public Task<Tag> GetTagIncludeProductsByTagId(int tagId);

        public ValueTask<bool> EditTagAsync(Tag tagModel);
        public ValueTask<bool> DeleteTag(Tag tagModel);
        public Task<bool> SaveAsync();
    }
}
