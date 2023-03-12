using ASP.Net_Meeting_18_Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels
{
    public class IndexCategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = default!;
        public SelectList? ParentCategorySL { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
