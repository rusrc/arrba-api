using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;

namespace Arrba.Repositories.Cache
{
    public class CachedPropertyRepository : PropertyRepository
    {
        const string propertyAll = "properties:all";

        public CachedPropertyRepository(DbArrbaContext context)
            : base(context) { }

        public override async Task<IEnumerable<Property>> GetAllAsync(Expression<Func<Property, bool>> predicate)
        {
            var key = propertyAll;
            var result = await CacheService.GetListAsync<Property>(key);

            if (result != null)
            {
                return result.Where(predicate.Compile());
            }

            result = await base.GetAllAsync();

            if (result != null)
            {
               await CacheService.SetListAsync(key, result);
            }

            return result.Where(predicate.Compile());
        }
    }
}
