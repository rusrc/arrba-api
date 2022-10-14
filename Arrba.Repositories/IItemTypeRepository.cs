using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;

namespace Arrba.Repositories
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        Task<IEnumerable<ItemType>> GetItemTypes(long categId);
    }
}