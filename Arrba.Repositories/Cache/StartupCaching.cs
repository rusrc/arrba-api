using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arrba.Domain.Models;
using Arrba.Services;
using Arrba.Services.Logger;
using AutoMapper;

namespace Arrba.Repositories.Cache
{
    public class StartupCaching : IStartupCaching
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;
        private readonly IMapper _mapper;
        public StartupCaching(IUnitOfWork unitOfWork, ILogService logService, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._log = logService;
            this._mapper = mapper;
        }

        public void CacheData()
        {
            this._log.Info("\n\n");
            this._log.Info("Start flush data");
            this._log.Info(CacheService.FlushDb());
            this._log.Info("End flush data");

            this._log.Info("Data caching start...");

            var categories = this._unitOfWork.CategoryRepository.GetAll();
            foreach (var category in categories)
            {
                this._unitOfWork.CategoryRepository.Get(category.ID);

                #region Cache vahicles
                var adVehicles = this._unitOfWork.AdVehicleRepository.GetAll(e => e.CategID == category.ID);
                if (!adVehicles.Any()) continue;

                var key = "vehicles:by-category-id:" + category.ID;
                var list = new RedisDictionary<long, AdVehicle>(key);

                var sw = new Stopwatch();
                sw.Start();
                this._log.Info("Start insert the adVehicles with category id " + category.ID);
                list.AddMultiple(adVehicles.Select(e => new KeyValuePair<long, AdVehicle>(e.ID, e)));
                this._log.Info($"End insert the adVehicles with category id {category.ID}, taken time: {sw.ElapsedMilliseconds}ms");
                sw.Stop();
                #endregion
            }


            //this._log.Info("Start getting data from redis...");
            //var sw1 = new Stopwatch();
            //sw1.Start();
            //var items = new RedisDictionary<long, AdVehicle>("vehicles:by-category-id:37");
            //IEnumerable<AdVehicle> vehicles = items.Values;
            //this._log.Info($"End getting data from redis..., count of vehicles: {vehicles.Count()}, taken time: {sw1.ElapsedMilliseconds}ms");

            this._log.Info("Data caching end...");
        }
    }
}
