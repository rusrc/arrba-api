using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemTypesController : Controller
    {
        private readonly DbArrbaContext _context;
        private readonly IMapper _mapper;

        public ItemTypesController(DbArrbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: ItemTypes
        public async Task<IActionResult> Index()
        {
            var categoryTypes = await _context
                .CategTypes
                .Include(ct => ct.Categ)
                .Include(ct => ct.ItemType).ToListAsync();

            var models = _mapper.Map<IEnumerable<TypeWithCategory>>(categoryTypes);

            return View(models.OrderBy(m => m.ItemTypeName));
        }

        // GET: ItemTypes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // GET: ItemTypes/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "ID", "Name");

            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,ItemTypeName,ItemTypeComment,ItemTypeStatus,ItemTypeWatchWeightStatus")] TypeWithCategoryCreate model)
        {
            var categType = await _context.CategTypes
                .Include(ct => ct.ItemType)
                .Include(ct => ct.Categ)
                .SingleOrDefaultAsync(ct => ct.ItemType.Name == model.ItemTypeName && ct.CategID == model.CategoryId);

            var typeExsits = categType != null;

            if (typeExsits)
            {
                ModelState.AddModelError("", $"Type '{model.ItemTypeName}' and category '{categType.Categ.Name}' already exists");
            }

            if (ModelState.IsValid)
            {
                var category = categType?.Categ ?? await _context.Categories.SingleOrDefaultAsync(c => c.ID == model.CategoryId);
                
                var newItemType = new ItemType
                {
                    Name = model.ItemTypeName,
                    Status = model.ItemTypeStatus,
                    Comment = model.ItemTypeComment,
                    WatchWeightStatus = model.ItemTypeWatchWeightStatus
                };

                var newCategType = new CategType
                {
                    Categ = category,
                    ItemType = newItemType
                };


                _context.Add(newCategType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "ID", "Name");

            return View(model);
        }

        // GET: ItemTypes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Comment,Status,WatchWeightStatus,NameMultiLangJson")] ItemType itemType)
        {
            if (id != itemType.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.ID))
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
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.ID == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // POST: ItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);
            _context.ItemTypes.Remove(itemType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemTypeExists(long id)
        {
            return _context.ItemTypes.Any(e => e.ID == id);
        }

    }
}
