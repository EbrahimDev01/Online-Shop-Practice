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
    public class Product
    {
        [Key]
        public int ProductId { set; get; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [MaxLength(150, ErrorMessage = ErrorMessage.MaxLength)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [StringLength(250, MinimumLength = 10, ErrorMessage = ErrorMessage.StringLength)]
        public string Descritption { set; get; }

        [Required(ErrorMessage = ErrorMessage.Required)]
        [MinLength(10, ErrorMessage = ErrorMessage.StringLength)]
        public string Explanation { get; set; }


        public Decimal Price { set; get; }

        public int QuantityInStok { set; get; }

        public DateTime CreateDateTime { set; get; }

        public int Views { set; get; }

        #region Relations

        [Required(ErrorMessage = ErrorMessage.Required)]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { set; get; }


        public virtual ICollection<Tag> Tags { set; get; } = new HashSet<Tag>();
        public virtual ICollection<CartItem> CartItems { set; get; } = new HashSet<CartItem>();
        public virtual ICollection<Comment> Comments { set; get; } = new HashSet<Comment>();
        public virtual ICollection<Image> Images { set; get; } = new HashSet<Image>();


        #endregion
    }
}
