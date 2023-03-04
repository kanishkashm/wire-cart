using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WireCart.Entities
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public bool IsActive { get; set; } = true;
        public IEnumerable<SubCategory> SubCategories { get; set; }
    }
}
