using System;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrba.Domain;
using Arrba.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arrba.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DbArrbaContext _context;
        private readonly IMapper _mapper;

        public UsersController(DbArrbaContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Create([Bind("Email,UserName,UserLastName,Password,PhoneNumber")] UserModelCreate user)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                await _userManager.CreateAsync(new User
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    RegistrationDate = now,
                    LastLogin = now,
                    UserStatus = UserAuthStatus.Authorized
                }, user.Password);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<UserModelEdit>(user);

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,Email,UserName,UserLastName,Password,PhoneNumber,RoleId")] UserModelEdit model)
        {
            var user = await _context.Users.FindAsync(model.UserId);

            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    user.UserStatus = model.UserStatus;
                    user.UserName = model.UserName;
                    user.UserLastName = model.UserLastName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    _context.Update(user);

                    var userRole = new IdentityUserRole<long>
                    {
                        UserId = model.UserId,
                        RoleId = model.RoleId
                    };

                    var currentRole = await _context.UserRoles.FindAsync(model.UserId, model.RoleId);
                    if (currentRole == null)
                    {
                        await _context.UserRoles.AddAsync(userRole);
                    }
                    else
                    {
                        _context.UserRoles.Update(userRole);
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.UserId))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View(model);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
