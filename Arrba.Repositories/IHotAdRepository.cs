using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arrba.Repositories
{
    public interface IHotAdRepository
    {
        Task<IEnumerable<object>> GetHotAds(long countryId, long regionId, long cityId);
    }
}