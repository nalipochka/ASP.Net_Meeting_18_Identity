using ASP.Net_Meeting_18_Identity.Data;

namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels
{
    public class PanelViewModel
    {
        public List<Product> Products { get; set; } = default!;
        public List<Photo> Photos { get; set; } = default!;
    }
}
