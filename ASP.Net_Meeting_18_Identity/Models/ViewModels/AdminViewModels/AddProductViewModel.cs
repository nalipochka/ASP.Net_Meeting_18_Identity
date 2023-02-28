using ASP.Net_Meeting_18_Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels
{
    public class AddProductViewModel
    {
        public Product product { get; set; } = default!;
        public SelectList? CategoriesSl { get; set; } = default!;
        public IEnumerable<IFormFile> Photos { get; set; } = default!;

    }
}
