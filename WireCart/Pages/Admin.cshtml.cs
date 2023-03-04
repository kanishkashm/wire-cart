using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WireCart.Entities;
using WireCart.Model;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminModel(
            ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository,
            IProductRepository productRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager
        )
        {
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public int PageSize = 5;

        public int SelectedResource { get; set; } = 1;
        public IEnumerable<Category> CategoryList { get; set; } = new List<Category>();
        public IEnumerable<SubCategory> SubCategoryList { get; set; } = new List<SubCategory>();
        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();

        public PaginationModel PageInfo { get; set; } = new PaginationModel();

        public async Task<IActionResult> OnGetAsync(int? resourceId, int p = 1, int s = 5)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role).ToList();
            var roles = new List<string>();

            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }
            if (!roles.Contains("Admin"))
            {
                return RedirectToPage("Product");
            }
            if (resourceId.HasValue)
            {
                SelectedResource = resourceId.Value;
            }
            if(SelectedResource == 1)
            {
                PageInfo.CurrentPage = p;
                PageInfo.Total = await _categoryRepository.GetTotalCatCount();
                var categories = await _categoryRepository.GetCategories(p, s);
                CategoryList = categories;
            }
            else if (SelectedResource == 2)
            {
                PageInfo.CurrentPage = p;
                PageInfo.Total = await _subCategoryRepository.GetTotalSubCatCount();
                var subCategories = await _subCategoryRepository.GetSubCategories(p, s);
                SubCategoryList = subCategories;
            }
            else if (SelectedResource == 3)
            {
                PageInfo.CurrentPage = p;
                PageInfo.Total = await _productRepository.GetTotalProductCount();
                var products = await _productRepository.GetProducts(p, s);
                ProductList = products;
            }

            return Page();
        }
    }
}
