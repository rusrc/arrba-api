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
    public class CachedSearchFormRepository : SearchFormRepository
    {
        const string vehicleByCategoryId = "vehicles:by-category-id:";
        private static Lazy<LogService> LogService => new Lazy<LogService>(() => new LogService());

        public CachedSearchFormRepository(DbArrbaContext ctx, ILogService logService)
            : base(ctx, logService) { }

        public override async Task<IEnumerable<AdVehicle>> GetAdsAsync(IQueryString queryString, long countryId, long regionId = 0, long cityId = 0)
        {
            var categoryId = queryString.CategID;
            var key = vehicleByCategoryId + categoryId;

            IEnumerable<AdVehicle> resultedList = await CacheService.GetListAsync<AdVehicle>(key);

            if (resultedList != null && queryString.IsQueryCached())
            {
                LogService.Value.Info("CachedSearchFormRepository GetAdsAsync() - cached data");
                return resultedList;
            }
            else
            {
                LogService.Value.Info("CachedSearchFormRepository GetAdsAsync() - not cached data");
                resultedList = await base.GetAdsAsync(queryString, countryId, regionId, cityId);
            }

            if (queryString.IsQueryCached() && resultedList != null)
            {
                var task = Task.Run(async () =>
                {
                    await CacheService.SetListAsync(key, resultedList);
                })
                .ContinueWith(async t =>
                {
                    if (t.IsFaulted)
                    {
                        await CacheService.DeleteKeyAsync(key);
                        LogService.Value.Error(t.Exception?.Message + " Error from task ", t.Exception);
                    }
                });
            }

            return resultedList;
        }
    }
}
