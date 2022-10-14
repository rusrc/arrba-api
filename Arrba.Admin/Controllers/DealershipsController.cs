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
    public class DealershipsController : Controller
    {
        private readonly DbArrbaContext _context;

        public DealershipsController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: Dealerships
        public async Task<IActionResult> Index()
        {
            var dbArrbaContext = _context.Dealerships.Include(d => d.City).Include(d => d.User);
            return View(await dbArrbaContext.ToListAsync());
        }

        // GET: Dealerships/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealerships
                .Include(d => d.City)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dealership == null)
            {
                return NotFound();
            }

            return View(dealership);
        }

        // GET: Dealerships/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "ID", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            var model = new Dealership
            {
                MoWorkTime = "9:00 - 20:00",
                TuWorkTime = "9:00 - 20:00",
                WeWorkTime = "9:00 - 20:00",
                ThWorkTime = "9:00 - 20:00",
                FrWorkTime = "9:00 - 20:00",
                SaWorkTime = "9:00 - 20:00",
                SuWorkTime = "9:00 - 20:00"
            };
            return View(model);
        }

        // POST: Dealerships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CityId,UserId,Name,Email,Address,SubwayStations,PhoneNumber,MapCoords,OfficialDealer,MoWorkTime,TuWorkTime,WeWorkTime,ThWorkTime,FrWorkTime,SaWorkTime,SuWorkTime")] Dealership dealership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dealership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "ID", "Name", dealership.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", dealership.UserId);
            return View(dealership);
        }

        // GET: Dealerships/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealerships.FindAsync(id);
            if (dealership == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "ID", "Name", dealership.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", dealership.UserId);
            return View(dealership);
        }

        // POST: Dealerships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CityId,UserId,Name,Email,Address,SubwayStations,PhoneNumber,MapCoords,OfficialDealer,MoWorkTime,TuWorkTime,WeWorkTime,ThWorkTime,FrWorkTime,SaWorkTime,SuWorkTime")] Dealership dealership)
        {
            if (id != dealership.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dealership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealershipExists(dealership.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "ID", "Name", dealership.CityId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", dealership.UserId);
            return View(dealership);
        }

        // GET: Dealerships/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = await _context.Dealerships
                .Include(d => d.City)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dealership == null)
            {
                return NotFound();
            }

            return View(dealership);
        }

        // POST: Dealerships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dealership = await _context.Dealerships.FindAsync(id);
            _context.Dealerships.Remove(dealership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DealershipExists(long id)
        {
            return _context.Dealerships.Any(e => e.Id == id);
        }
    }
}
