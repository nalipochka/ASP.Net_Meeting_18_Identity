using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "The name of the category must be specified!")]
        public string Title { get; set; } = default!;

        public int? ParentCategoryId { get; set; }

        public SelectList? ParentCategory { get; set; } = default!;
    }
}
