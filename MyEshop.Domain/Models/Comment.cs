using MyEshop.Domain.ConstsDomain.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class Comment
    {
        public int CommentId { set; get; }
        
        public string Title { get; set; }

        [MinLength(10)]
        public string Body { set; get; }
        
        public DateTime CreateDateTime { get; set; }

        #region Relations

        public int ProductId { set; get; }
        public virtual Product Product { set; get; }

        public string ApplicationUserId { set; get; }
        public virtual ApplicationUser ApplicationUser { set; get; }

        #endregion
    }
}
