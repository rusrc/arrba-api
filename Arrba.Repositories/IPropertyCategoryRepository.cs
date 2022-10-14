using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface IPropertyCategoryRepository: IRepository<PropertyCateg>
    {
        Task<IEnumerable<PropertyCateg>> GetAllAsync(long categoryId);
        Task<IEnumerable<PropertyCateg>> GetAllAsync(long categoryId, bool addCheckBoxs);
    }
}
