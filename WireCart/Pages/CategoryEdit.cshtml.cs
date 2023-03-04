using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class CategoryEditModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryEditModel(
            IWebHostEnvironment environment,
            ICategoryRepository categoryRepository
        )
        {
            _environment = environment;
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Category Category { get; set; }

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public string PreviousFileName { get; set; }

        public async Task OnGetAsync(int catId)
        {
            Category = await _categoryRepository.GetCategoryById(catId);
            PreviousFileName = Category.ImageName;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FileUpload != null && FileUpload.Length > 0)
            {
                var fileExtension = Path.GetExtension(FileUpload.FileName);
                var fileName = $"{Guid.NewGuid()}.{fileExtension}";
                string filePath = Path.Combine(_environment.WebRootPath, "images", "category", fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FileUpload.CopyToAsync(fileStream);
                }
                Category.ImageName = fileName;
            }
            else
            {
                Category.ImageName = Category.ImageName;
            }

            await _categoryRepository.UpdateAsync(Category);
            return RedirectToPage("Admin", new { resourceId = 1 });
        }
    }
}
