using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbArrbaContext context) : base(context)
        {
        }

        public virtual async Task<Category> GetAsync(string alias)
        {
            return await GetAsync(c => c.Alias == alias);
        }

        public IQueryable<Category> GetCategories()
        {
            return _context.Categories.Include(c => c.SuperCateg);
        }
    }
}
