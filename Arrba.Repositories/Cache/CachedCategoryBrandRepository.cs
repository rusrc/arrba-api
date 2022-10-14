using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Logger;

namespace Arrba.Repositories.Cache
{
    public class CachedCategoryBrandRepository : CategoryBrandRepository
    {
        const string categoryAlias = "brands:categoryid:";
        private static Lazy<LogService> LogService => new Lazy<LogService>(() => new LogService());

        public CachedCategoryBrandRepository(DbArrbaContext context) 
            : base(context)
        {
        }

        public override async Task<IEnumerable<CategBrand>> GetAllAsync(long categoryId)
        {
            var key = categoryAlias + categoryId;
            var categoryBrands = CacheService.GetData<IEnumerable<CategBrand>>(key);

            if (categoryBrands != null)
            {
                return categoryBrands;
            }

            categoryBrands = await base.GetAllAsync(categoryId);

            if (categoryBrands != null)
            {
                await CacheService.SetDataAsync(key, categoryBrands);
            }

            return categoryBrands;         
        }
    }
}
