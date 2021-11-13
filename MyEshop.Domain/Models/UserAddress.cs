using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Domain.Models
{
    public class UserAddress
    {
        public string State { set; get; }
        public string PostalCard { set; get; }
        public string City { set; get; }
        public string PostalAddress { set; get; }
        public string Plaque { set; get; }
        public string UnitNumber { set; get; }


        public string RecipientFirstName { set; get; }
        public string RecipientLastName { set; get; }
        public string RecipientPhoneNumber { set; get; }

        #region Relations

        public string ApplicationUserId { set; get; }
        public ApplicationUser ApplicationUser { set; get; }

        #endregion
    }
}
