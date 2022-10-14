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
    public class PropertiesController : Controller
    {
        private readonly DbArrbaContext _context;

        public PropertiesController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var dbArrbaContext = _context.Properties.Include(p => p.PropertyGroup);
            return View(await dbArrbaContext.ToListAsync());
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var pproperty = await _context.Properties
                .Include(p => p.PropertyGroup)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pproperty == null)
            {
                return NotFound();
            }

            return View(pproperty);
        }

        // GET: Properties/Create
        public IActionResult Create()
        {
            ViewData["PropertyGroupID"] = new SelectList(_context.PropertyGroups, "ID", "Name");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PropertyGroupID,Name,Description,UnitMeasure,ControlType,ActiveStatus,NameMultiLangJson")] Property pproperty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pproperty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PropertyGroupID"] = new SelectList(_context.PropertyGroups, "ID", "Name", pproperty.PropertyGroupID);
            return View(pproperty);
        }

        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pproperty = await _context.Properties.FindAsync(id);
            if (pproperty == null)
            {
                return NotFound();
            }
            ViewData["PropertyGroupID"] = new SelectList(_context.PropertyGroups, "ID", "Name", pproperty.PropertyGroupID);
            return View(pproperty);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,PropertyGroupID,Name,Description,UnitMeasure,ControlType,ActiveStatus,NameMultiLangJson")] Property pproperty)
        {
            if (id != pproperty.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pproperty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(pproperty.ID))
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
            ViewData["PropertyGroupID"] = new SelectList(_context.PropertyGroups, "ID", "Name", pproperty.PropertyGroupID);
            return View(pproperty);
        }

        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pproperty = await _context.Properties
                .Include(p => p.PropertyGroup)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pproperty == null)
            {
                return NotFound();
            }

            return View(pproperty);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pproperty = await _context.Properties.FindAsync(id);
            _context.Properties.Remove(pproperty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(long id)
        {
            return _context.Properties.Any(e => e.ID == id);
        }
    }
}
