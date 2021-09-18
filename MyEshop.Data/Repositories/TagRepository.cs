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

        public IAsyncEnumerable<Tag> GetTagsAsync() => _dbContext.Tags;

        public IEnumerable<Tag> GetTagsByIds(IEnumerable<int> ids)
            => _dbContext.Tags.Where(t => ids.Any(id => t.TagId == id));

        public IQueryable<Tag> GetTagsProductByProductId(int ProductId)
            => _dbContext.Tags.Where(tag => tag.Products.Any(product => product.ProductId == ProductId));


    }
}
