using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ISearchFormRepository
    {
        Task<IEnumerable<AdVehicle>> GetAdsAsync(IQueryString queryString, long countryId, long regionId = 0, long cityId = 0);
        Task<int> GetCountAsync(IQueryString queryString, long countryId, long regionId = 0L, long cityId = 0L);
    }
}