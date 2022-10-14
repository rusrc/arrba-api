using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Logger;

namespace Arrba.Repositories.Cache
{
    public class CachedItemTypeRepository : ItemTypeRepository
    {
        string _key = "types:categoryid:";
        string _typekey = "type:Id:";

        public CachedItemTypeRepository(DbArrbaContext context, ILogService log) 
            : base(context, log)
        {
        }

        public override async Task<IEnumerable<ItemType>> GetItemTypes(long categId)
        {
            _key = _key + categId;
            var types = CacheService.GetData<IEnumerable<ItemType>>(_key);

            if (types != null)
            {
                return types;
            }

            types = await base.GetItemTypes(categId);

            if (types != null)
            {
                await CacheService.SetDataAsync(_key, types);
            }

            return types;
        }

        public override async Task<ItemType> GetAsync(object typeId)
        {
            _typekey = _typekey + (long)typeId;
            var type = CacheService.GetData<ItemType>(_typekey);

            if (type != null)
            {
                return type;
            }

            type = await base.GetAsync((long)typeId);

            if (type != null)
            {
                await CacheService.SetDataAsync(_typekey, type);
            }

            return type;
        }
    }
}
