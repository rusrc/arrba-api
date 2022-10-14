using System.Collections.Generic;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;

namespace Arrba.Repositories.Cache
{
    public class CachedPropertyCategoryRepository : PropertyCategoryRepository
    {
        const string propertyNameKey = "property_category_list:categoryid:";
        const string propertyNameKeyWithCheckBoxs = "property_category_list:check_boxes:categoryid:";

        public CachedPropertyCategoryRepository(DbArrbaContext context)
            : base(context) { }

        public override async Task<IEnumerable<PropertyCateg>> GetAllAsync(long categoryId)
        {
            var key = propertyNameKey + categoryId;
            var properties = CacheService.GetData<IEnumerable<PropertyCateg>>(key);

            if (properties != null)
            {
                return properties;
            }

            properties = await base.GetAllAsync(categoryId);

            if (properties != null)
            {
                await CacheService.SetDataAsync(key, properties);
            }

            return properties;
        }

        public override async Task<IEnumerable<PropertyCateg>> GetAllAsync(long categoryId, bool addCheckBoxs)
        {
            var key = propertyNameKeyWithCheckBoxs + categoryId;

            var properties = CacheService.GetData<IEnumerable<PropertyCateg>>(key);

            if (properties != null)
            {
                return properties;
            }

            properties = await base.GetAllAsync(categoryId, addCheckBoxs);

            if (properties != null)
            {
                await CacheService.SetDataAsync(key, properties);
            }

            return properties;
        }
    }
}
