using System.Linq;
using System.Threading.Tasks;
using Arrba.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BrandsController : Controller
    {
        private readonly DbArrbaContext _context;

        public BrandsController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "ID", "Name");

            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,BrandName,CategoryId,Status,WatchWeightStatus")] BrandWithCategoryCreate model)
        {
            var categBrand = await _context.CategBrands
                .Include(ct => ct.Brand)
                .Include(ct => ct.Categ)
                .SingleOrDefaultAsync(ct => ct.Brand.Name == model.BrandName && ct.CategID == model.CategoryId);

            var brandExsits = categBrand != null;

            if (brandExsits)
            {
                ModelState.AddModelError("", $"Brand: '{model.BrandName}' and category '{categBrand.Categ.Name}' already exists");
            }

            if (ModelState.IsValid)
            {
                var category = categBrand?.Categ ?? await _context.Categories.SingleOrDefaultAsync(c => c.ID == model.CategoryId);

                var newBrand = new Brand
                {
                    Name = model.BrandName,
                    Status = model.Status,
                    WatchWeightStatus = model.WatchWeightStatus
                };

                var newCategBrand = new CategBrand
                {
                    Categ = category,
                    Brand = newBrand
                };

                _context.Add(newCategBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "ID", "Name");

            return View(model);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Status,WatchWeightStatus,LikeValue,LikeCount")] Brand brand)
        {
            if (id != brand.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.ID))
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
            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.ID == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(long id)
        {
            return _context.Brands.Any(e => e.ID == id);
        }
    }
}
