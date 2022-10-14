using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class PropertyCategoryRepository : Repository<PropertyCateg>, IPropertyCategoryRepository
    {
        public PropertyCategoryRepository(DbArrbaContext context)
            : base(context) { }


        public virtual async Task<IEnumerable<PropertyCateg>> GetAllAsync(long categoryId)
        {
            var props = await _context.PropertyCategs
                .Include(p => p.Property)
                .Include(p => p.Property.SelectOptions)
                .Where(p => p.CategID == categoryId &&
                        (
                            p.AddToFilter == AddToFilter.AddedToFilterOnly ||
                            p.AddToFilter == AddToFilter.AddedToEveryWhere) &&
                            p.AddToFilter != AddToFilter.RemovedFromEveryWhere
                        )
                .ToListAsync();

            return props;
        }

        public virtual async Task<IEnumerable<PropertyCateg>> GetAllAsync(long categoryId, bool addCheckBoxs)
        {
            var props = await _context.PropertyCategs
                .Include(p => p.Property)
                .Include(p => p.Property.SelectOptions)
                .Include(p => p.Property.PropertyGroup)
                .Where(p => p.CategID == categoryId)
                .ToListAsync();

            return props;
        }
    }
}
