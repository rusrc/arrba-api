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
    public class ItemModelsController : Controller
    {
        private readonly DbArrbaContext _context;

        public ItemModelsController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: ItemModels
        public async Task<IActionResult> Index()
        {
            var dbArrbaContext = _context.ItemModels.Include(i => i.Brand).Include(i => i.Categ).Include(i => i.ItemType);
            return View(await dbArrbaContext.ToListAsync());
        }

        // GET: ItemModels/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.ItemModels
                .Include(i => i.Brand)
                .Include(i => i.Categ)
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (itemModel == null)
            {
                return NotFound();
            }

            return View(itemModel);
        }

        // GET: ItemModels/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID");
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID");
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ID", "ID");
            return View();
        }

        // POST: ItemModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategID,ItemTypeID,BrandID,Name,Status,WatchWeightStatus,LikeValue,LikeCount")] ItemModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", itemModel.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", itemModel.CategID);
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ID", "ID", itemModel.ItemTypeID);
            return View(itemModel);
        }

        // GET: ItemModels/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.ItemModels.FindAsync(id);
            if (itemModel == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", itemModel.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", itemModel.CategID);
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ID", "ID", itemModel.ItemTypeID);
            return View(itemModel);
        }

        // POST: ItemModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,CategID,ItemTypeID,BrandID,Name,Status,WatchWeightStatus,LikeValue,LikeCount")] ItemModel itemModel)
        {
            if (id != itemModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemModelExists(itemModel.ID))
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", itemModel.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", itemModel.CategID);
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ID", "ID", itemModel.ItemTypeID);
            return View(itemModel);
        }

        // GET: ItemModels/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemModel = await _context.ItemModels
                .Include(i => i.Brand)
                .Include(i => i.Categ)
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (itemModel == null)
            {
                return NotFound();
            }

            return View(itemModel);
        }

        // POST: ItemModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var itemModel = await _context.ItemModels.FindAsync(id);
            _context.ItemModels.Remove(itemModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemModelExists(long id)
        {
            return _context.ItemModels.Any(e => e.ID == id);
        }
    }
}
