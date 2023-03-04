using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WireCart.Entities
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
