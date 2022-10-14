using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<IEnumerable<City>> GetAllAsync(long countryId);
        Task<City> GetAsync(string alias);
        Task<IEnumerable<City>> GetByWeightAsync(int top, long countryId, bool isActiveOnly = false);
    }
}
