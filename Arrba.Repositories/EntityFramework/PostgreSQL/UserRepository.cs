using System;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Exceptions;
using Arrba.Services.Logger;
using Microsoft.AspNetCore.Identity;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ILogService _log;
        private readonly UserManager<User> _userManager;

        public UserRepository(DbArrbaContext context, UserManager<User> userManager, ILogService log)
            : base(context)
        {
            _log = log;
            _userManager = userManager;
        }

        public async Task<bool> CheckUser(string email, string password)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> Add(User user, string password)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).Aggregate((prev, next) => prev + "\n" + next);
                throw new BusinessLogicException(errors);
            }

            return result.Succeeded;
        }

        public async Task<User> GetAsync(string email, bool loadPhones = false, bool loadBalance = false)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (loadPhones) await _context.Entry(user).Collection(c => c.UserPhones).LoadAsync();
            if (loadBalance) await _context.Entry(user).Reference(p => p.Balance).LoadAsync();

            return user;
        }

        /// <exception cref="BusinessLogicException"></exception>
        public async Task<User> CreateUnauthorizedUserOrGetExistedAsync(string email)
        {
            var user = (await _createUnauthorizedUserOrGetExistedAsync(email)).Item1;
            _context.Entry(user).Collection(c => c.UserPhones).Load();

            return user;
        }

        private async Task<Tuple<User, IdentityResult>> _createUnauthorizedUserOrGetExistedAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new NullReferenceException("Email пустой");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new User
                {
                    Email = email,
                    UserName = email,
                    RegistrationDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    UserStatus = UserAuthStatus.Anonymous
                };
                var result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded)
                {
                    return Tuple.Create(newUser, result);
                }

                throw new BusinessCriticalLogicException
                (
                    result.Errors.Select(e => e.Description).Aggregate((a, b) => a + "\n" + b)
                );
            }

            return Tuple.Create(user, (IdentityResult) null);
        }
    }
}
