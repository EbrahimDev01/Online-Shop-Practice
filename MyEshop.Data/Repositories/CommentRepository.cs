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
    public class CommentRepository : ICommentRepository
    {
        private readonly MyEshopDBContext _dbContext;

        public CommentRepository(MyEshopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool DeleteCommentsByProductId(int productId)
        {
            try
            {
                var comments = GetCommentsByProductId(productId);

                return DeleteComments(comments);
            }
            catch
            {
                return false;
            }
        }

        public int GetCommentCountProductByProductId(int productId)
            => _dbContext.Comments.Where(comment => comment.ProductId == productId).Count();

        public IEnumerable<Comment> GetCommentsByProductId(int productId)
            => _dbContext.Comments.Where(comment => comment.ProductId == productId);

        public bool DeleteComments(IEnumerable<Comment> comments)
        {
            try
            {
                _dbContext.Comments.RemoveRange(comments);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
