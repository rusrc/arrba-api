using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface IUserPhoneRepository : IRepository<UserPhone>
    {
        Task<UserPhone> GetByVehicleIdAsync(long adVehicleId);
        Task<IEnumerable<UserPhone>> GetAllAsync(long adVehicleId);
    }
}
