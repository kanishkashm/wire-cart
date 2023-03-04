using System.ComponentModel.DataAnnotations.Schema;

namespace WireCart.Entities
{
    [Table("UserReview")]
    public class UserReview
    {
        public int Id { get; set; }

        public int Rate { get; set; }
        public string Comment { get; set; }
        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; }
    }
}
