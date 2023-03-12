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

        public async Task<IActionResult> Index()
        {
            //List<Product> context = await dbContext.Propducts.Include(p => p.Photos).ToListAsync();
            List<Product> contextProducts = await dbContext.Products.ToListAsync();
            List<Photo> contextPhoto = await dbContext.Photos.ToListAsync();
            IndexViewModel vM = new IndexViewModel()
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
                    string webRoot = hostEnvironment.WebRootPath+ "/images/";
                    string filePath = Path.Combine(webRoot, fileName);
                    using(FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await item.CopyToAsync(fileStream);
                    }
                    Photo photo = new Photo()
                    {
                        FileName = item.FileName,
                        PhotoUrl = "/images/" + fileName,
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
                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
                foreach (var error in ModelState.Values.SelectMany(t => t.Errors))
                {
                    logger.LogError(error.ErrorMessage);
                }
            return View(vM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.Products == null)
            {
                return NotFound();
            }

            var product = await dbContext.Products
                .Include(p => p.Category)
                .Include(t => t.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.Products == null)
            {
                return NotFound();
            }

            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(dbContext.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Count,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(product);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(dbContext.Categories, "Id", "Title", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Products == null)
            {
                return NotFound();
            }

            var product = await dbContext.Products
                .Include(p => p.Category)
                .Include(f => f.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Products == null)
            {
                return Problem("Entity set 'ShopDbContext.Products'  is null.");
            }
            var product = await dbContext.Products.FindAsync(id);
            if (product != null)
            {
                dbContext.Products.Remove(product);
            }

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return dbContext.Products.Any(e => e.Id == id);
        }
        //[HttpPost]
        //public async Task<IActionResult> getChildCategories(int? id)
        //{
        //    if(id != null)
        //    {
        //        var childCategories = await dbContext.Categories
        //            .Where(t=>t.ParentCategoryId == id).ToListAsync();
        //        return PartialView(childCategories);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
