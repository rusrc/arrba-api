using System.Linq;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ISuperCategoryRepository : IRepository<SuperCategory>
    {
        IQueryable<SuperCategory> GetSuperCategs();
    }
}
