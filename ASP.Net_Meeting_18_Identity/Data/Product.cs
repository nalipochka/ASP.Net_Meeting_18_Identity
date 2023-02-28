using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.Net_Meeting_18_Identity.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public double Price { get; set; }
        public int Count { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Photo>? Photos { get; set; }
    }
}
