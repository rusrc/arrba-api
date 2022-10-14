using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Logger;

namespace Arrba.Repositories.Cache
{
    public class CachedCityRepository : CityRepository
    {
        const string citiesAll = "cities:all";
        const string cityId = "city:id:";
        const string citiesByCategory = "cities:categoryId:";
        const string citiesByAlias = "cities:alias:";
        const string citiesOrderedByWeight = "cities:orderedByWeight";

        private ILogService _logService;

        public CachedCityRepository(DbArrbaContext context, ILogService logService)
            : base(context)
        {
            _logService = logService;
        }

        public override async Task<IEnumerable<City>> GetAllAsync(long countryId)
        {
            var key = citiesByCategory + countryId;
            var result = await CacheService.GetListAsync<City>(key);

            if (result != null)
            {
                return result;
            }

            result = await base.GetAllAsync(countryId);

            if (result != null)
            {
                await CacheService.SetListAsync(key, result);
            }

            return result;
        }

        public override async Task<IEnumerable<City>> GetAllAsync()
        {
            var key = citiesAll;
            var result = await CacheService.GetListAsync<City>(key);

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

        public override async Task<City> GetAsync(object key)
        {
#if DEBUG
            _logService.Info($"Cached method called... {nameof(CachedCityRepository)}");
#endif
            var cachedKey = cityId + (long)key;
            var city = CacheService.GetData<City>(cachedKey);

            if (city != null)
            {
                return city;
            }

            city = await base.GetAsync(key);

            if (city != null)
            {
                await CacheService.SetDataAsync(cachedKey, city);
            }

            return city;
        }

        public override async Task<City> GetAsync(string alias)
        {
            var key = citiesByAlias + alias;
            var result = CacheService.GetData<City>(key);

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

        public override async Task<IEnumerable<City>> GetByWeightAsync(int top, long countryId, bool isActiveOnly = false)
        {
            var key = citiesOrderedByWeight;
            var result = await CacheService.GetListAsync<City>(key);

            if (result != null)
            {
                return result;
            }

            result = await base.GetByWeightAsync(top, countryId);

            if (result != null)
            {
                await CacheService.SetListAsync(key, result);
            }

            return result;
        }
    }
}
