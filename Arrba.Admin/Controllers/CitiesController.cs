using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using PagedList.Core;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly DbArrbaContext _context;
        private readonly int _pageSize;

        public CitiesController(DbArrbaContext context, IConfiguration configuration)
        {
            _context = context;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        // GET: Cities
        public IActionResult Index(int? page)
        {
            var query = _context.Cities
                .Include(c => c.Country)
                .Include(c => c.Region);

            var paggedList = new PagedList<City>(query.AsNoTracking(), page ?? 1, _pageSize);

            return View(paggedList);
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Region)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name");
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,RegionID,CountryID,Name,Alias,Weight,Status,NameMultiLangJson")] City city)
        {
            if (ModelState.IsValid)
            {
                city.Alias = Transliteration.Generate(city.Name);

                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name", city.RegionID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "ID", "Name", city.CountryID);
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name", city.RegionID);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,RegionID,CountryID,Name,Alias,Weight,Status,NameMultiLangJson")] City city)
        {
            if (id != city.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.ID))
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
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name", city.RegionID);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Region)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(long id)
        {
            return _context.Cities.Any(e => e.ID == id);
        }
    }
}
