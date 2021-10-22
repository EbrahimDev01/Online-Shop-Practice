using Microsoft.EntityFrameworkCore;
using MyEshop.Data.Context;
using MyEshop.Domain.Interfaces;
using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly MyEshopDBContext _dbContext;

        public TagRepository(MyEshopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                await _dbContext.Tags.AddAsync(tag);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IAsyncEnumerable<Tag> GetTagsAsync() => _dbContext.Tags;

        public IEnumerable<Tag> GetTagsByIds(IEnumerable<int> ids)
            => _dbContext.Tags.Where(t => ids.Any(id => t.TagId == id));

        public IQueryable<Tag> GetTagsProductByProductId(int ProductId)
            => _dbContext.Tags.Where(tag => tag.Products.Any(product => product.ProductId == ProductId));

        public Task<bool> IsExistTagByTitle(string title)
            => _dbContext.Tags.AnyAsync(tag => tag.Title == title);
    }
}
