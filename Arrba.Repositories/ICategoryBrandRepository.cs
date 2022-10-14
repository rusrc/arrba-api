using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ICategoryBrandRepository: IRepository<CategBrand>
    {
        Task<IEnumerable<CategBrand>> GetAllAsync(long categoryId);
    }
}