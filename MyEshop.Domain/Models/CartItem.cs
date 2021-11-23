namespace MyEshop.Domain.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int NumberOfOrders { set; get; }


        #region Relations

        public int CartId { get; set; }
        public virtual Cart Cart { set; get; }


        public int ProductId { get; set; }
        public virtual Product Product { set; get; }

        #endregion
    }
}
