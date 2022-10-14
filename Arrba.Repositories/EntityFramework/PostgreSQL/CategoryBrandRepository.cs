using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class CategoryBrandRepository : Repository<CategBrand>, ICategoryBrandRepository
    {
        public CategoryBrandRepository(DbArrbaContext context) 
            : base(context)
        {
        }

        public virtual async Task<IEnumerable<CategBrand>> GetAllAsync(long categoryId)
        {
            var result = await _context.CategBrands
                            .Include(b => b.Brand)
                            .Where(b => b.CategID == categoryId && b.Brand.Status == ActiveStatus.active)
                            .ToListAsync();

            return result;
        }
    }
}
