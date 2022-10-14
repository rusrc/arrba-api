using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SuperCategoriesController : Controller
    {
        private readonly DbArrbaContext _context;

        public SuperCategoriesController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: SuperCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.SuperCategories.ToListAsync());
        }

        // GET: SuperCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superCategory = await _context.SuperCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (superCategory == null)
            {
                return NotFound();
            }

            return View(superCategory);
        }

        // GET: SuperCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SuperCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Alias,Order,Name,Status,SuperCategType,NameMultiLangJson")] SuperCategory superCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(superCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(superCategory);
        }

        // GET: SuperCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superCategory = await _context.SuperCategories.FindAsync(id);
            if (superCategory == null)
            {
                return NotFound();
            }
            return View(superCategory);
        }

        // POST: SuperCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Alias,Order,Name,Status,SuperCategType,NameMultiLangJson")] SuperCategory superCategory)
        {
            if (id != superCategory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(superCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperCategoryExists(superCategory.ID))
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
            return View(superCategory);
        }

        // GET: SuperCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superCategory = await _context.SuperCategories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (superCategory == null)
            {
                return NotFound();
            }

            return View(superCategory);
        }

        // POST: SuperCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var superCategory = await _context.SuperCategories.FindAsync(id);
            _context.SuperCategories.Remove(superCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuperCategoryExists(long id)
        {
            return _context.SuperCategories.Any(e => e.ID == id);
        }
    }
}
