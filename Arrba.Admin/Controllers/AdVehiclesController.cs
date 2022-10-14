using System.Linq;
using System.Threading.Tasks;
using Arrba.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;
using Microsoft.Extensions.Configuration;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdVehiclesController : Controller
    {
        private readonly DbArrbaContext _context;
        private readonly int _pageSize;

        public AdVehiclesController(DbArrbaContext context, IConfiguration configuration)
        {
            _context = context;
            _pageSize = configuration.GetValue<int>("PageSize");
        }

        // GET: AdVehicles
        public IActionResult Index(int? page)
        {
            var query = _context.AdVehicles
                .Include(a => a.Brand)
                .Include(a => a.Categ)
                .Include(a => a.City)
                .Include(a => a.Currency)
                .Include(a => a.Dealership)
                .Include(a => a.Model)
                .Include(a => a.Region)
                .Include(a => a.SuperCateg)
                .Include(a => a.Type)
                .Include(a => a.User);

            var paggedList = new PagedList<AdVehicle>(query.AsNoTracking(), page ?? 1, 15);

            return View(paggedList);
        }

        // GET: AdVehicles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adVehicle = await _context.AdVehicles
                .Include(a => a.Brand)
                .Include(a => a.Categ)
                .Include(a => a.City)
                .Include(a => a.Currency)
                .Include(a => a.Dealership)
                .Include(a => a.Model)
                .Include(a => a.Region)
                .Include(a => a.SuperCateg)
                .Include(a => a.Type)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (adVehicle == null)
            {
                return NotFound();
            }

            return View(adVehicle);
        }

        // GET: AdVehicles/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "Name");
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "Name");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name");
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "Name");
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Name");
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "Name");
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name");
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name");
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: AdVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdVehicleCreateEdit adVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "Name", adVehicle.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "Name", adVehicle.CategID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name", adVehicle.CityID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "Name", adVehicle.CurrencyID);
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Name", adVehicle.DealershipId);
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "Name", adVehicle.ModelID);
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name", adVehicle.RegionID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name", adVehicle.SuperCategID);
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "Name", adVehicle.TypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName", adVehicle.UserID);
            return View(adVehicle);
        }

        // GET: AdVehicles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adVehicle = await _context.AdVehicles.FindAsync(id);
            if (adVehicle == null)
            {
                return NotFound();
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "Name", adVehicle.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "Name", adVehicle.CategID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name", adVehicle.CityID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "Name", adVehicle.CurrencyID);
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Name", adVehicle.DealershipId);
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "Name", adVehicle.ModelID);
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name", adVehicle.RegionID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name", adVehicle.SuperCategID);
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "Name", adVehicle.TypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName", adVehicle.UserID);
            return View(adVehicle);
        }

        // POST: AdVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,UserID,SuperCategID,BrandID,TypeID,CityID,RegionID,CountryID,CurrencyID,Year,Price,MinimalPrice,NewModelName,InstalmentSelling,CustomsCleared,HotSelling,ExchangePossible,Comment,Title,CategID,ModelID,DealershipId,ImgJson,Description,FolderImgName,MapJsonCoord,ViewCount,ImgExists,AddDate,DateExpired,LastModified,AdStatus,IsAutoUpdatable,ModirationStatus,CommentRestriction,ModelVerification,Condition")] AdVehicle adVehicle)
        {
            if (id != adVehicle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adVehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdVehicleExists(adVehicle.ID))
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "Name", adVehicle.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "Name", adVehicle.CategID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "Name", adVehicle.CityID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "Name", adVehicle.CurrencyID);
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Name", adVehicle.DealershipId);
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "Name", adVehicle.ModelID);
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "Name", adVehicle.RegionID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "Name", adVehicle.SuperCategID);
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "Name", adVehicle.TypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "UserName", adVehicle.UserID);
            return View(adVehicle);
        }

        // GET: AdVehicles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adVehicle = await _context.AdVehicles
                .Include(a => a.Brand)
                .Include(a => a.Categ)
                .Include(a => a.City)
                .Include(a => a.Currency)
                .Include(a => a.Dealership)
                .Include(a => a.Model)
                .Include(a => a.Region)
                .Include(a => a.SuperCateg)
                .Include(a => a.Type)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (adVehicle == null)
            {
                return NotFound();
            }

            return View(adVehicle);
        }

        // POST: AdVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var adVehicle = await _context.AdVehicles.FindAsync(id);
            _context.AdVehicles.Remove(adVehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdVehicleExists(long id)
        {
            return _context.AdVehicles.Any(e => e.ID == id);
        }
    }
}
