using ASP.Net_Meeting_18_Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Net_Meeting_18_Identity.ViewComponents
{
    [ViewComponent]
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly ShopDbContext context;

        public CategoriesMenuViewComponent(ShopDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? currentCategory)
        {
            List<string> categoryNames = await context.Products.Include(t=>t.Category)
                .Select(t => t.Category!.Title).Distinct().ToListAsync();
            return View(new Tuple<List<string>, string?>(categoryNames, currentCategory));
        }
    }
}
