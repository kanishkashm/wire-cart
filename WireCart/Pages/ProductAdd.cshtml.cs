using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class ProductAddModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        //private readonly ISubCategoryRepository _subCategoryRepository;

        public ProductAddModel(
            IWebHostEnvironment environment,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository
            //ISubCategoryRepository subCategoryRepository
        )
        {
            _environment = environment;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            //_subCategoryRepository = subCategoryRepository;
        }

        [BindProperty]
        public Product Product { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public SelectList CategoryOptions { get; set; }

        public SelectList SubCategoryOptions { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        [BindProperty]
        public IFormFile FileUpload { get; set; }

        public async Task OnGetAsync()
        {
            var categories = await _categoryRepository.GetCategoriesWithSubCategory(x => x.IsActive);
            Categories = categories;
            CategoryOptions = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (FileUpload != null && FileUpload.Length > 0)
            {
                var fileExtension = Path.GetExtension(FileUpload.FileName);
                var fileName = $"{Guid.NewGuid()}.{fileExtension}";
                string filePath = Path.Combine(_environment.WebRootPath, "images", "product", fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FileUpload.CopyToAsync(fileStream);
                }
                Product.ImageFile = fileName;
            }

            await _productRepository.AddAsync(Product);
            return RedirectToPage("Admin", new { resourceId = 3 });
        }
    }
}
