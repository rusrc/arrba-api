using System;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Logger;

namespace Arrba.Repositories.Cache
{
    public class CachedBrandRepository : BrandRepository
    {
        const string brandNameKey = "brand:name:";
        public CachedBrandRepository(DbArrbaContext context, ILogService logService)
            : base(context, logService) { }

        public override async Task<Brand> GetAsync(string brandName)
        {
            var key = brandNameKey + brandName;
            var brand = CacheService.GetData<Brand>(key);

            if (brand != null)
            {
                return brand;
            }

            brand = await base.GetAsync(brandName);

            if (brand != null)
            {
                await CacheService.SetDataAsync(key, brand);
            }

            return brand;
        }

        public override void Add(Brand item)
        {
            throw new NotImplementedException();
            //base.Add(item);
        }

        public override void Update(Brand item)
        {
            throw new NotImplementedException();
            //base.Update(item);
        }

        public override void Remove(Brand item)
        {
            throw new NotImplementedException();
            //base.Remove(item);
        }
    }
}
