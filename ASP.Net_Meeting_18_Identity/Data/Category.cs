namespace ASP.Net_Meeting_18_Identity.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category>? ChildCategories { get; set; }
        public List<Product>? Products { get; set; }
    }
}
