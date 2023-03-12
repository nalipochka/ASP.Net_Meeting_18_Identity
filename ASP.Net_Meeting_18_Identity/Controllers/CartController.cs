using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Extansions;
using ASP.Net_Meeting_18_Identity.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Meeting_18_Identity.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopDbContext context;

        public CartController(ShopDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(string? returnUrl, string returnUrlToShop)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            Cart cart = GetCart();

            return View(new Tuple<Cart, string>(cart, returnUrlToShop));
        }
        public async Task<IActionResult> AddToCart(int id, string returnUrl)
        {
            Cart cart = GetCart();
            Product? product = await context.Products.FindAsync(id);
            if (product != null)
            {
                cart.AddToCart(product, 1);
                HttpContext.Session.Set("cart", cart.CartItems);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cart cart = GetCart();
            Product? product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            cart.RemoveFromCart(product);
            HttpContext.Session.Set("cart", cart.CartItems);
            return RedirectToAction("Index");
        }
        public Cart GetCart()
        {
            IEnumerable<CartItem>? cartItems = HttpContext.Session.Get<IEnumerable<CartItem>>("cart");
            Cart? cart = null;
            if (cartItems == null)
            {
                cart = new Cart();
            }
            else
            {
                cart = new Cart(cartItems!);
            }

            return cart;
        }
    }
}
