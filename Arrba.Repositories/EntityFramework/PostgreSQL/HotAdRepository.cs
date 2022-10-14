using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    using SETTTS = Constants.SETTINGS;
    public class HotAdRepository : IHotAdRepository
    {
        readonly DbArrbaContext _ctx;
        public HotAdRepository(DbArrbaContext ctx)
        {
            this._ctx = ctx;
        }

        // public async Task<IEnumerable<HotAdMV>> GetHotAds 
        public async Task<IEnumerable<object>> GetHotAds
        (long countryId, long regionId, long cityId)
        {
            var hotAds = from a in _ctx.AdVehicles.Include(a => a.City)
                         join s in _ctx.AdVehicleServiceStores on a.ID equals s.AdVehicleID
                         join m in _ctx.ItemModels on a.ModelID equals m.ID
                         where s.ServiceType == ServiceEnum.IsHot
                         orderby s.BoughtDate descending
                         select a;

            List<AdVehicle> result = null;
            if (SETTTS.HotAdLimit > 0)
                result = await hotAds.Take(SETTTS.HotAdLimit).ToListAsync();
            else
                result = await hotAds.ToListAsync();


            return result.Select(e => new HotAdMV
            {
                ID = e.ID,
                CityName = e.City.Name,
                Price = e.Price,
                ModelName = "empty",
                CategID = e.CategID,
                FolderImgName = e.FolderImgName,
                Year = e.Year,
                // ImgJson = e.MainImgMiddle,
                ImgJsonObject = null // e.ImgJsonObject
            }).ToList();
        }
    }
}
