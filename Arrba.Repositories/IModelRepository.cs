using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface IModelRepository : IRepository<ItemModel>
    {
        Task<ItemModel> GetAsync(string modelName, long brandId);
        Task<IEnumerable<ItemModel>> GetModels(long categId, long brandId);
    }
}
