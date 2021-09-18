using MyEshop.Data.Context;
using MyEshop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyEshopDBContext _dbContext;

        public CommentRepository(MyEshopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetCommentCountProductByProductId(int productId)
            => _dbContext.Comments.Where(comment => comment.ProductId == productId).Count();
    }
}
