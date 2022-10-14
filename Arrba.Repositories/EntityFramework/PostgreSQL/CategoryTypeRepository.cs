using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class CategoryTypeRepository : Repository<CategType>, ICategoryTypeRepository
    {
        public CategoryTypeRepository(DbArrbaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<CategType>> GetAllAsync()
        {
            return await _context.CategTypes.Include(ct => ct.ItemType).ToListAsync();
        }
    }
}
