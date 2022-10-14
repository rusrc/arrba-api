using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "Admin,Manager")]
    public class AdVehiclesForManagerController : Controller
    {
        private readonly DbArrbaContext _context;

        public AdVehiclesForManagerController(DbArrbaContext context)
        {
            _context = context;
        }

        // GET: AdVehiclesForManager
        public async Task<IActionResult> Index()
        {
            var dbArrbaContext = _context.AdVehicles
                .Include(a => a.Brand)
                .Include(a => a.Categ).Include(a => a.City)
                .Include(a => a.Currency)
                .Include(a => a.Dealership)
                .Include(a => a.Model)
                .Include(a => a.Region)
                .Include(a => a.SuperCateg)
                .Include(a => a.Type)
                .Include(a => a.User);

            return View(await dbArrbaContext.ToListAsync());
        }

        // GET: AdVehiclesForManager/Details/5
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

        // GET: AdVehiclesForManager/Create
        public IActionResult Create()
        {
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID");
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID");
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "ID");
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "ID");
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Id");
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "ID");
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "ID");
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "ID");
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AdVehiclesForManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,SuperCategID,BrandID,TypeID,CityID,RegionID,CountryID,CurrencyID,Year,Price,MinimalPrice,NewModelName,InstalmentSelling,CustomsCleared,HotSelling,ExchangePossible,Comment,Title,CategID,ModelID,DealershipId,ImgJson,Description,FolderImgName,MapJsonCoord,ViewCount,ImgExists,AddDate,DateExpired,LastModified,AdStatus,IsAutoUpdatable,ModirationStatus,CommentRestriction,ModelVerification,Condition")] AdVehicle adVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adVehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", adVehicle.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", adVehicle.CategID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "ID", adVehicle.CityID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "ID", adVehicle.CurrencyID);
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Id", adVehicle.DealershipId);
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "ID", adVehicle.ModelID);
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "ID", adVehicle.RegionID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "ID", adVehicle.SuperCategID);
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "ID", adVehicle.TypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", adVehicle.UserID);
            return View(adVehicle);
        }

        // GET: AdVehiclesForManager/Edit/5
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", adVehicle.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", adVehicle.CategID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "ID", adVehicle.CityID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "ID", adVehicle.CurrencyID);
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Id", adVehicle.DealershipId);
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "ID", adVehicle.ModelID);
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "ID", adVehicle.RegionID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "ID", adVehicle.SuperCategID);
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "ID", adVehicle.TypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", adVehicle.UserID);
            return View(adVehicle);
        }

        // POST: AdVehiclesForManager/Edit/5
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "ID", "ID", adVehicle.BrandID);
            ViewData["CategID"] = new SelectList(_context.Categories, "ID", "ID", adVehicle.CategID);
            ViewData["CityID"] = new SelectList(_context.Cities, "ID", "ID", adVehicle.CityID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "ID", "ID", adVehicle.CurrencyID);
            ViewData["DealershipId"] = new SelectList(_context.Dealerships, "Id", "Id", adVehicle.DealershipId);
            ViewData["ModelID"] = new SelectList(_context.ItemModels, "ID", "ID", adVehicle.ModelID);
            ViewData["RegionID"] = new SelectList(_context.Regions, "ID", "ID", adVehicle.RegionID);
            ViewData["SuperCategID"] = new SelectList(_context.SuperCategories, "ID", "ID", adVehicle.SuperCategID);
            ViewData["TypeID"] = new SelectList(_context.ItemTypes, "ID", "ID", adVehicle.TypeID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", adVehicle.UserID);
            return View(adVehicle);
        }

        // GET: AdVehiclesForManager/Delete/5
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

        // POST: AdVehiclesForManager/Delete/5
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
