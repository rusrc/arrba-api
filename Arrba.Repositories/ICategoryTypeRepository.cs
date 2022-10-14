using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ICategoryTypeRepository
    {
        Task<IEnumerable<CategType>> GetAllAsync();
    }
}