using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Models.ViewModels.AdminViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ASP.Net_Meeting_18_Identity.Controllers
{
    [Authorize(Roles ="admin")]
    public class CategoriesController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly ILogger _logger;

        public CategoriesController(ShopDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<CategoriesController>();
        }

        public async Task<IActionResult> Index(int? parentCategoryId)
        {
            IQueryable<Category> categories = _context.Categories
                .Include(c => c.ParentCategory);
            if (parentCategoryId != null)
                categories = categories.Where(p => p.ParentCategoryId == parentCategoryId);

            IEnumerable<Category> tempCategories =await categories.ToListAsync();

            SelectList parentCategorySL = new(await _context.Categories.ToListAsync(),
                nameof(Category.Id),
                nameof(Category.Title),
                parentCategoryId);
            IndexCategoryViewModel vm = new IndexCategoryViewModel
            {
                Categories = tempCategories,
                ParentCategorySL = parentCategorySL,
                ParentCategoryId = parentCategoryId
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                SelectList parentCategoty = new(await _context.Categories.ToListAsync(),
                    nameof(Category.Id),
                    nameof(Category.Title),
                    vm.ParentCategoryId);
                foreach (var error in ModelState.Values.SelectMany(t => t.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return View(vm);
            }
            var category = new Category
            {
                Title = vm.Title,
                ParentCategoryId = vm.ParentCategoryId
            };
            Category createdCategory = category;
            if (createdCategory.ParentCategoryId == 0)
            {
                createdCategory.ParentCategoryId = null;
            }
            _context.Add(createdCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title", category.ParentCategoryId);
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title", category.ParentCategoryId);
            return View(category);
        }
     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ShopDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
