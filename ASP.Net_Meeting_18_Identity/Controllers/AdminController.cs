using ASP.Net_Meeting_18_Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Net_Meeting_18_Identity.Controllers
{
    [Authorize(Roles ="admin,manager")]
    public class AdminController : Controller
    {
        private readonly ShopDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger logger;

        public AdminController(ShopDbContext dbContext, IWebHostEnvironment hostEnvironment, ILoggerFactory logger)
        {
            this.dbContext = dbContext;
            this.hostEnvironment = hostEnvironment;
            this.logger = logger.CreateLogger<AdminController>();
        }

        public async Task<IActionResult> PanelAsync()
        {
            //List<Product> context = await dbContext.Propducts.Include(p => p.Photos).ToListAsync();
            List<Product> contextProducts = await dbContext.Propducts.ToListAsync();
            List<Photo> contextPhoto = await dbContext.Photos.ToListAsync();
            PanelViewModel vM = new PanelViewModel()
            {
                Photos = contextPhoto,
                Products = contextProducts
            };
            return View(vM);
        }

        public async Task<IActionResult> AddProductAsync()
        {
            AddProductViewModel viewModel = new AddProductViewModel()
            {
                CategoriesSl = new SelectList(await dbContext.Categories.ToListAsync(), "Id", "Title", 0)
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductViewModel vM)
        {
            if(ModelState.IsValid)
            {
                List<Photo> photos = new List<Photo>();
                foreach (var item in vM.Photos)
                {
                    //string fileName = Path.GetFileName(item.FileName);
                    string fileName = item.FileName;
                    string webRoot = hostEnvironment.WebRootPath;
                    string filePath = Path.Combine(webRoot, fileName);
                    using(FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await item.CopyToAsync(fileStream);
                    }
                    Photo photo = new Photo()
                    {
                        FileName = item.FileName,
                        PhotoUrl = fileName,
                        ProductId = vM.product.Id,
                        Product = vM.product
                    };
                    photos.Add(photo);
                    //await dbContext.Photos.AddAsync(photo);
                    //await dbContext.SaveChangesAsync();
                }
                Product product = vM.product;
                //product.Photos = photos;
                await dbContext.Photos.AddRangeAsync(photos);
                await dbContext.Propducts.AddAsync(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Panel", "Admin");
            }
                foreach (var error in ModelState.Values.SelectMany(t => t.Errors))
                {
                    logger.LogError(error.ErrorMessage);
                }
            return View(vM);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Categories.AddAsync(category);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Panel", "Admin");
            }
            return View(category);
        }
    }
}
