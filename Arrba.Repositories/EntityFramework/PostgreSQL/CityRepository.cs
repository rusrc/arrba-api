using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DbArrbaContext context)
            : base(context)
        {
        }

        public virtual async Task<IEnumerable<City>> GetAllAsync(long countryId)
        {
            return await _context.Cities
                .Where(city => city.CountryID == countryId)
                .OrderBy(city => city.Name)
                .ToListAsync();
        }

        public virtual async Task<City> GetAsync(string alias)
        {
            return await _context.Cities.SingleOrDefaultAsync(c => c.Alias == alias);
        }

        public virtual async Task<IEnumerable<City>> GetByWeightAsync(int top, long countryId, bool isActiveOnly = false)
        {
            var query = _context.Cities
                .Where(city => city.CountryID == countryId);

            if (isActiveOnly)
            {
                query = query.Where(c => c.Status == ActiveStatus.active);
            }

            return await query
                .OrderByDescending(city => city.Weight)
                .Take(top)
                .ToListAsync();
        }
    }
}
