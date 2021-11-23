using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NationalCode { set; get; }
        public DateTime Birthday { set; get; }

        #region Relations

        public int? UserImageId { set; get; }
        public virtual Image UserImage { set; get; }

        public virtual ICollection<Comment> UserComments { set; get; } = new HashSet<Comment>();

        public virtual ICollection<Cart> UserCarts { set; get; } = new HashSet<Cart>();

        public virtual ICollection<UserAddress> UserAddresses { set; get; } = new HashSet<UserAddress>();

        public int UserWalletId { set; get; }
        public virtual UserWallet UserWallet { set; get; }
       
        #endregion
    }
}
