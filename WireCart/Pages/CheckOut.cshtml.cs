using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WireCart.Entities;
using WireCart.Repositories.Interfaces;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        public CheckOutModel(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        [BindProperty]
        public Order Order { get; set; }

        public Cart Cart { get; set; } = new Cart();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _cartRepository.GetCartByUserName("test");
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await _cartRepository.GetCartByUserName("test");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = "test";
            Order.TotalPrice = Cart.TotalPrice;

            await _orderRepository.CheckOut(Order);
            await _cartRepository.ClearCart("test");
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}