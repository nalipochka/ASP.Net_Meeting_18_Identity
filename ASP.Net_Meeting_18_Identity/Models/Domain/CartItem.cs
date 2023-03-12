using ASP.Net_Meeting_18_Identity.Data;

namespace ASP.Net_Meeting_18_Identity.Models.Domain
{
    public class CartItem
    {
        public Product Product { get; set; } = default!;
        public int Count { get; set; }
    }
}
