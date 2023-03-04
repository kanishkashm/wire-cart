using System.ComponentModel.DataAnnotations.Schema;

namespace WireCart.Entities
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }

                return totalprice;
            }
        }
        public CartStatus Status { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
