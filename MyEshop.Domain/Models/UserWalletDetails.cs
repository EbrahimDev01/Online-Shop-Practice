using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class UserWalletDetails
    {
        public TypeOfTransaction Transaction { set; get; }
        public decimal AmountMoney { set; get; }
        public DateTime TransactionDate { set; get; }
        public ReasonOfTransaction ReasonTransaction { set; get; }

        public int UserWalletId { set; get; }
        public virtual UserWallet UserWallet { set; get; }

        public int? CartId { set; get; }
        public virtual Cart Cart { set; get; }
    }

    public enum TypeOfTransaction
    {
        Deposit,
        Withdraw
    }

    public enum ReasonOfTransaction
    {
        ChargeWallet,
        AboutInvoice,
        All = ChargeWallet | AboutInvoice
    }
}
