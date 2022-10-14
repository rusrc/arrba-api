using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;
using Arrba.Services;
using Arrba.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class ItemTypeRepository : Repository<ItemType>, IItemTypeRepository
    {
        private readonly ILogService _log;
        public ItemTypeRepository(DbArrbaContext context, ILogService log)
            : base(context)
        {
            _log = log;
        }

        // TODO replace in redis repository
        public virtual async Task<IEnumerable<ItemType>> GetItemTypes(long categId)
        {
            // var key = $"types:categoryid:{categId}";

            var types = await _context.CategTypes
                 .Include(ct => ct.ItemType)
                 .Where(ct => ct.CategID == categId)
                .Select(ct => ct.ItemType)
                .ToListAsync();

            return types;
        }
    }
}
