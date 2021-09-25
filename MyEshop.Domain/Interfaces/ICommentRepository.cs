using MyEshop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Interfaces
{
    public interface ICommentRepository
    {
        public int GetCommentCountProductByProductId(int productId);

        public bool DeleteCommentsByProductId(int productId);

        public IEnumerable<Comment> GetCommentsByProductId(int productId);

        public bool DeleteComments(IEnumerable<Comment> comments);
    }
}
