using ASP.Net_Meeting_18_Identity.Data;

namespace ASP.Net_Meeting_18_Identity.Models.Domain
{
    public class Cart
    {
        List<CartItem> cartItems;
        public IEnumerable<CartItem> CartItems => cartItems;

        public Cart(IEnumerable<CartItem> cartItems) 
        {
            this.cartItems = cartItems.ToList();
        }
        public Cart()
        {
            cartItems = new List<CartItem>();
        }
        public void AddToCart(Product product, int count)
        {
            CartItem? item = CartItems.FirstOrDefault(t => t.Product.Id == product.Id);
            if (item != null)
            {
                item.Count += count;
            }
            else
            {
                cartItems.Add(new CartItem { Product = product, Count = count });
            }
        }
        public void RemoveFromCart(Product product)
        {
            //CartItem? cartItemcartItem = CartItems.FirstOrDefault(t => t.Product.Id == product.Id);
            cartItems.RemoveAll(t => t.Product.Id == product.Id);
        }
        public void Clear()
        {
            cartItems.Clear();
        }
        public double TotalPrice => cartItems.Sum(t => t.Product.Price * t.Count);
    }
}
