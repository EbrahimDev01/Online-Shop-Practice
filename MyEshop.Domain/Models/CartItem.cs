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
    public class CartItem
    {
        [Key]
        public int CartItemsId { get; set; }
        public int NumberOfOrders { set; get; }


        #region Relations

        [Required(ErrorMessage = ErrorMessage.Required)]
        public int CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public virtual Cart Cart { set; get; }


        [Required(ErrorMessage = ErrorMessage.Required)]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { set; get; }

        #endregion
    }
}
