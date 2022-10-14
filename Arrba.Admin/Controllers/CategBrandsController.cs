using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using PagedList.Core;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategBrandsController : Controller
    {
        private readonly DbArrbaContext _context;
        private readonly int _pageSize;

        public CategBrandsController(DbArrbaContext context, IConfiguration configuration)
        {
            _context = context;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        // GET: CategBrands
        public IActionResult Index(int? page)
        {
            var query = _context.CategBrands
                .Include(c => c.Brand)
                .Include(c => c.Categ);

            var paggedList = new PagedList<CategBrand>(query.AsNoTracking(), page ?? 1, _pageSize);

            return View(paggedList);
        }

        // GET: CategBrands/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categBrand = await _context.CategBrands
                .Include(c => c.Brand)
                .Include(c => c.Categ)
                .FirstOrDefaultAsync(m => m.CategID == id);
            if (categBrand == null)
            {
                return NotFound();
            }

            return View(categBrand);
        }

        // GET: CategBrands/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "Name");
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "Name");
            return View();
        }

        // POST: CategBrands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategID,BrandID")] CategBrand categBrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "Name", categBrand.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "Name", categBrand.CategID);
            return View(categBrand);
        }

        // GET: CategBrands/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categBrand = await _context.CategBrands.FindAsync(id);
            if (categBrand == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", categBrand.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", categBrand.CategID);
            return View(categBrand);
        }

        // POST: CategBrands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CategID,BrandID")] CategBrand categBrand)
        {
            if (id != categBrand.CategID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategBrandExists(categBrand.CategID))
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", categBrand.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", categBrand.CategID);
            return View(categBrand);
        }

        // GET: CategBrands/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categBrand = await _context.CategBrands
                .Include(c => c.Brand)
                .Include(c => c.Categ)
                .FirstOrDefaultAsync(m => m.CategID == id);
            if (categBrand == null)
            {
                return NotFound();
            }

            return View(categBrand);
        }

        // POST: CategBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var categBrand = await _context.CategBrands.FindAsync(id);
            _context.CategBrands.Remove(categBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategBrandExists(long id)
        {
            return _context.CategBrands.Any(e => e.CategID == id);
        }
    }
}
