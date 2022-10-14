using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;

namespace Arrba.Repositories.Cache
{
    public class CachedAdVehicleRepository : AdVehicleRepository
    {
        const string vehicleIdKey = "vehicle:id:";

        public CachedAdVehicleRepository(
            DbArrbaContext context, 
            IQueryString queryString, 
            ISearchFormRepository searchFormRepository, 
            ICurrencyRepository currencyRepository)
            : base(context, queryString, searchFormRepository, currencyRepository) { }

        public override async Task<AdVehicle> GetAsync(long adVehicleId)
        {
            var key = vehicleIdKey + adVehicleId;
            var vahicle = CacheService.GetData<AdVehicle>(key);

            if (vahicle != null)
            {
                return vahicle;
            }

            vahicle = await base.GetAsync(adVehicleId);

            if (vahicle != null)
            {
                await CacheService.SetDataAsync(key, vahicle);
            }

            return vahicle;
        }
    }
}
