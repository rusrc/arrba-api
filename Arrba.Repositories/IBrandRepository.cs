using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.DTO;

namespace Arrba.Repositories
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<Brand> GetAsync(string brandName);

        Task<IEnumerable<CategoryWithBrandsDto>> GetCategoriesWithBrandsAsync(int vehiclesCount, Expression<Func<AdVehicle, bool>> selectByCityIdPredicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}
