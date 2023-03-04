using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WireCart.Entities;
using WireCart.Extensions;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public ProductModel(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository
        )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public IEnumerable<Category> CategoryList { get; set; } = new List<Category>();
        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? categoryId)
        {
            CategoryList = await _categoryRepository.GetCategories();

            if (categoryId.HasValue)
            {
                var categories = await _categoryRepository.GetCategoriesWithSubCategory(x => x.Id == categoryId);
                if(categories != null)
                {
                    var subCatIds = categories.First().SubCategories.Select(y => y.Id);
                    ProductList = await _productRepository.GetProducts(x => subCatIds.Contains(x.SubCategoryId));
                }
                SelectedCategory = CategoryList.FirstOrDefault(c => c.Id == categoryId.Value)?.Name;
            }
            else
            {
                ProductList = await _productRepository.GetProducts();
            }

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