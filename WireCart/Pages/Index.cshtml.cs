using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WireCart.Entities;
using WireCart.Extensions;
using WireCart.Model;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public IndexModel(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository
         )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public List<CategoryProductViewModel> CategoriesProducts { get; set; } = new List<CategoryProductViewModel>();
        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();

        public async Task<IActionResult> OnGetAsync()
        {

            var categories = await _categoryRepository.GetCategoriesWithSubCategory(x => x.IsActive);
            foreach (var category in categories)
            {
                var subCatIds = category.SubCategories.Select(y => y.Id);
                var catProducts = await _productRepository.GetProducts(x => subCatIds.Contains(x.SubCategoryId));
                var categoryProducts = new CategoryProductViewModel
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    Products = catProducts.Take(4)
                };
                if (catProducts.Any())
                {
                    CategoriesProducts.Add(categoryProducts);
                }
            }
            ProductList = await _productRepository.GetProducts();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var myCart = HttpContext.Session.Get<Cart>("MyCart");
            if (myCart == null)
            {
                var username = HttpContext.User.Identity.Name;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                myCart = new Cart
                {
                    UserId = userId,
                    UserName = username,
                    Items = new List<CartItem>()
                };
            }
            // check product already in the cart
            if (myCart.Items.Any(x => x.ProductId == productId))
            {
                var productItem = myCart.Items.FirstOrDefault(x => x.ProductId == productId);
                productItem.Quantity += 1;
            }
            else
            {
                var product = await _productRepository.GetProductById(productId);
                myCart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Color = "Black",
                    Price = product.Price,
                    Product = product,
                    Quantity = 1
                });
            }

            HttpContext.Session.Set<Cart>("MyCart", myCart);
            return RedirectToPage("Cart");
        }
    }
}
