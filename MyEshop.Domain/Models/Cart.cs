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
        public int CartId { get; set; }
        public bool CartIsActive { set; get; }
        public DateTime? DateTimeCompleteCart { set; get; }

        #region Relations

        public virtual ICollection<CartItem> CartItems { set; get; } = new HashSet<CartItem>();

        public string ApplicationUserId { set; get; }
        public virtual ApplicationUser ApplicationUser { set; get; }

        public virtual ICollection<UserWalletDetails> UserWalletDetails { set; get; } = new HashSet<UserWalletDetails>();

        #endregion
    }
}
