using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class SubCategoryAddModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SubCategoryAddModel(
            IWebHostEnvironment environment,
            ISubCategoryRepository subCategoryRepository,
            ICategoryRepository categoryRepository
        )
        {
            _environment = environment;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public SubCategory SubCategory { get; set; }

        //public IEnumerable<Category> Categories { get; set; }
        public SelectList Options { get; set; }

        //[BindProperty]
        //public int SelectedCategory { get; set; }

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public async Task OnGet()
        {
            var categories = await _categoryRepository.GetCategories(x => x.IsActive);
            Options = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (FileUpload != null && FileUpload.Length > 0)
            {
                var fileExtension = Path.GetExtension(FileUpload.FileName);
                var fileName = $"{Guid.NewGuid()}.{fileExtension}";
                string filePath = Path.Combine(_environment.WebRootPath, "images", "sub_category", fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FileUpload.CopyToAsync(fileStream);
                }
                SubCategory.ImageName = fileName;
            }
            
            await _subCategoryRepository.AddAsync(SubCategory);
            return RedirectToPage("Admin", new { resourceId = 2});
        }

    }
}
