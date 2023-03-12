using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Models;
using ASP.Net_Meeting_18_Identity.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASP.Net_Meeting_18_Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDbContext context;

        public HomeController(ILogger<HomeController> logger, ShopDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index(string? category)
        {
            IQueryable<Product> products =context.Products.Include(t=>t.Category).Include(t=>t.Photos);
            if(category!=null)
            {
                products = products.Where(t => t.Category!.Title == category);
            }
            //automapper!!!!
            HomeIndexViewModel vM = new HomeIndexViewModel()
            {
                Products = await products.ToListAsync(),
                Category = category
            };
            return View(vM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}