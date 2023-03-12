namespace ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels
{
    public class AddCategoryViewModel
    {
        public string Title { get; set; } = default!;
        public int? ParentCategoryId { get; set; }
    }
}
