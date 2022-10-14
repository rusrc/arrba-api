using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;

namespace Arrba.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetAsync(string alias);
        IQueryable<Category> GetCategories();
    }
}
