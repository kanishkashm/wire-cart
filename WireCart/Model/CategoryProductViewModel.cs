using WireCart.Entities;

namespace WireCart.Model
{
    public class CategoryProductViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
