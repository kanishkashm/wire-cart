using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryModel(
            ICategoryRepository categoryRepository
            )
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> CategoryList { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGetAsync()
        {
            CategoryList = await _categoryRepository.GetCategories();

            return Page();
        }
    }
}
