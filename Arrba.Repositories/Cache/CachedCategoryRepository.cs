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
    public class CachedCategoryRepository : CategoryRepository
    {
        const string categoryAlias = "category:alias:";
        const string categoryId = "category:id:";
        const string categoryAll = "category:all";

        public CachedCategoryRepository(DbArrbaContext ctx)
            : base(ctx) { }

        public override async  Task<Category> GetAsync(string alias)
        {
            var key = categoryAlias + alias;
            var result = CacheService.GetData<Category>(key);

            if (result != null)
            {
                return result;
            }

            result = await base.GetAsync(alias);

            if (result != null)
            {
               await CacheService.SetDataAsync(key, result);
            }

            return result;
        }


        public override Category Get(object key)
        {
            var redisKey = categoryId + (long)key;
            var result = CacheService.GetData<Category>(redisKey);

            if (result != null)
            {
                return result;
            }

            result = base.Get((long)key);

            if (result != null)
            {
                CacheService.SetDataAsync(redisKey, result);
            }

            return result;
        }

        public override async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate)
        {
            var key = categoryAll;
            var result = await CacheService.GetListAsync<Category>(key);

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
