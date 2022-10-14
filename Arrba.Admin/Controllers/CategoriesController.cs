using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DbArrbaContext _context;

        public CategoriesController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string sortOrder)
        {
            // https://docs.microsoft.com/ru-ru/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AliasSortParm = sortOrder == "Alias" ? "alias_asc" : "";
            ViewBag.StatusSort = sortOrder == "Status" ? "status_asc" : "status_desc";

            var categories = from s in _context.Categories
                            .Include(c => c.CategGroup)
                            .Include(c => c.SuperCateg)
                            select s;

            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(s => s.Name);
                    break;
                case "alias_asc":
                    categories = categories.OrderBy(s => s.Alias);
                    break;
                case "status_asc":
                    categories = categories.OrderBy(s => s.Status);
                    break;
                case "status_desc":
                    categories = categories.OrderByDescending(s => s.Status);
                    break;
                default:
                    categories = categories.OrderBy(s => s.Name);
                    break;
            }

            return View(await categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.CategGroup)
                .Include(c => c.SuperCateg)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["CategGroupID"] = new SelectList(_context.CategGroups, "ID", "Name");
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SuperCategID,CategGroupID,Alias,HideModelField,NameMultiLangSingularJson,Name,FileName,Status,NameMultiLangJson")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.Alias = Helpers.Transliteration.Generate(category.Name);
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategGroupID"] = new SelectList(_context.CategGroups, "ID", "Name", category.CategGroupID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name", category.SuperCategID);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["CategGroupID"] = new SelectList(_context.CategGroups, "ID", "Name", category.CategGroupID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name", category.SuperCategID);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,SuperCategID,CategGroupID,Alias,HideModelField,NameMultiLangSingularJson,Name,FileName,Status,NameMultiLangJson")] Category category)
        {
            if (id != category.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.Alias = Helpers.Transliteration.Generate(category.Name);
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
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
            ViewData["CategGroupID"] = new SelectList(_context.CategGroups, "ID", "Name", category.CategGroupID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name", category.SuperCategID);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.CategGroup)
                .Include(c => c.SuperCateg)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(long id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}
