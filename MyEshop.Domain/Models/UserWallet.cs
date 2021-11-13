using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class UserWallet
    {
        public decimal WalletBalance { set; get; }

        public virtual ICollection<UserWalletDetails> UserWalletDetails { set; get; }

        public virtual ApplicationUser ApplicationUser { set; get; }

    }
}
