using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;

namespace Arrba.Repositories.Cache
{
    public class CachedCountryRepository : CountryRepository
    {
        const string countriesAll = "countries:all:";
        const string countryId = "country:id:";
        public CachedCountryRepository(DbArrbaContext context)
            : base(context)
        {
        }

        public override async Task<Country> GetAsync(object key)
        {
            var cachedKey = countryId + (long)key;
            var country = CacheService.GetData<Country>(cachedKey);

            if (country != null)
            {
                return country;
            }

            country = await base.GetAsync(key);

            if (country != null)
            {
                await CacheService.SetDataAsync(cachedKey, country);
            }

            return country;
        }

        public override async Task<IEnumerable<Country>> GetAllAsync()
        {
            var key = countriesAll;
            var countries = await CacheService.GetListAsync<Country>(key);

            if (countries != null)
            {
                return countries;
            }

            countries = await base.GetAllAsync();

            if (countries != null)
            {
               await CacheService.SetListAsync(key, countries);
            }

            return countries;
        }
    }
}
