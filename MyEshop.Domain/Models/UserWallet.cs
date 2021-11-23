using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class UserWallet
    {
        public int UserWalletId { set; get; }

        public decimal WalletBalance { set; get; }

        public virtual ICollection<UserWalletDetails> UserWalletDetails { set; get; } = new HashSet<UserWalletDetails>();

        public virtual ApplicationUser ApplicationUser { set; get; }

    }
}
