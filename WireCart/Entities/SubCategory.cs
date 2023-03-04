using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WireCart.Entities
{
    [Table("SubCategory")]
    public class SubCategory
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
