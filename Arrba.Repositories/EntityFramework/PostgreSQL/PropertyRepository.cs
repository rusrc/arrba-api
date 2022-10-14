using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        public PropertyRepository(DbArrbaContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Property>> 
            GetAllAsync(bool addOptions, Expression<Func<Property, bool>> predicate)
        {
            if (addOptions)
            {
                return await _context
                    .Properties
                    .Include(p => p.SelectOptions)
                    .Where(predicate)
                    .ToListAsync();
            }
            return await base.GetAllAsync();
        }
    }
}
