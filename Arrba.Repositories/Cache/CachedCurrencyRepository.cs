using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;

namespace Arrba.Repositories.Cache
{
    public class CachedCurrencyRepository : CurrencyRepository
    {
        const string currencyAll = "currency:all";
        public CachedCurrencyRepository(DbArrbaContext context) 
            : base(context)
        {
        }

        public override async Task<IEnumerable<Currency>> GetAllAsync()
        {
            var key = currencyAll;
            var result = await CacheService.GetListAsync<Currency>(key);

            if (result != null)
            {
                return result;
            }

            result = await base.GetAllAsync();

            if (result != null)
            {
               await CacheService.SetListAsync(key, result);
            }

            return result;
        }
    }
}
