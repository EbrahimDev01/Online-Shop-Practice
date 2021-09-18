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
        [Key]
        public int CommentId { set; get; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(150, ErrorMessage = ErrorMessage.MaxLength)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [StringLength(250, MinimumLength = 10, ErrorMessage = ErrorMessage.StringLength)]
        public string Body { set; get; }

        public DateTime CreateDateTime { get; set; }

        #region Relations

        [Required(ErrorMessage = ErrorMessage.Required)]
        public int ProductId { set; get; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { set; get; }

        #endregion
    }
}
