using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WireCart.Entities;
using WireCart.Extensions;
using WireCart.Repositories.Interfaces;

namespace WireCart.Pages
{
    public class CartModel : PageModel
    {
        private readonly ICartRepository _cartRepository;

        public CartModel(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Cart Cart { get; set; } = new Cart();

        public IActionResult OnGet()
        {
            var myCart = HttpContext.Session.Get<Cart>("MyCart");
            if (myCart != null)
            {
                Cart = myCart;
            }           

            return Page();
        }

        public IActionResult OnPostRemoveToCart(int productId)
        {
            var myCart = HttpContext.Session.Get<Cart>("MyCart");
            if (myCart != null)
            {
                var cartItem = myCart.Items.FirstOrDefault(x => x.ProductId == productId);
                myCart.Items.Remove(cartItem);
                HttpContext.Session.Set<Cart>("MyCart", myCart);
            }
            return RedirectToPage();
        }
    }
}