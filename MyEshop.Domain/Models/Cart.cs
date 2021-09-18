using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public bool CartIsActive { set; get; }
        public DateTime? DateTimeCompleteCart { set; get; }

        #region Relations

        public virtual ICollection<CartItem> CartItems { set; get; } = new HashSet<CartItem>();

        #endregion
    }
}
