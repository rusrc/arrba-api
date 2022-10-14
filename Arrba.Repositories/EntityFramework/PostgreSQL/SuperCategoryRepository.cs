using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class SuperCategoryRepository : Repository<SuperCategory>, ISuperCategoryRepository
    {
        private readonly ILogService _log;
        public SuperCategoryRepository(DbArrbaContext context, ILogService log)
            : base(context)
        {
            _log = log;
        }

        public new virtual async Task<IEnumerable<SuperCategory>> GetAllAsync(Expression<Func<SuperCategory, bool>> predicate)
        {
            var result = await base._context.SuperCategories
                .Include(c => c.Categories)
                .Where(predicate).ToListAsync();

            return result;
        }
        
        public IQueryable<SuperCategory> GetSuperCategs()
        {
            var result = _context.SuperCategories;
                //.Include(sc => sc.Categs);
                //.Include(sc => sc.Categs.Select(c => c.AdVehicles))
                //.Include(sc => sc.Categs.Select(c => c.AdVehicles.Select(ad => ad.Brand)));

            return result;
        }
    }
}
