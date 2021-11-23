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
        public int ProductId { set; get; }

        public string Title { get; set; }

        [MinLength(10)]
        public string Descritption { set; get; }

        [MinLength(10)]
        public string Explanation { get; set; }

        public Decimal Price { set; get; }

        public int QuantityInStok { set; get; }

        public DateTime CreateDateTime { set; get; }

        public int Views { set; get; }

        #region Relations

        public int CategoryId { get; set; }
        public virtual Category Category { set; get; }

        public virtual ICollection<Tag> Tags { set; get; } = new HashSet<Tag>();
        public virtual ICollection<CartItem> CartItems { set; get; } = new HashSet<CartItem>();
        public virtual ICollection<Comment> Comments { set; get; } = new HashSet<Comment>();
        public virtual ICollection<Image> Images { set; get; } = new HashSet<Image>();


        #endregion
    }
}
