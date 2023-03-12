using ASP.Net_Meeting_18_Identity.Data;

namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.HomeViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; } = default!;
        public string? Category { get; set; }
    }
}
