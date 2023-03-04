using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WireCart.Entities;
using WireCart.Extensions;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public ProductDetailModel(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public Product Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            Product = await _productRepository.GetProductById(productId.Value);
            if (Product == null)
            {
                return NotFound();
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