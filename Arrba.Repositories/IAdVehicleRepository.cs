using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Arrba.Repositories.EntityFramework.PostgreSQL;

namespace Arrba.Repositories
{
    public interface IAdVehicleRepository : IRepository<AdVehicle>
    {
        IQueryable<AdVehicle> GetAsync(long userId, long categoryId, Enums.RoomSorter sorter);

        /// <summary>
        /// Return single item
        /// </summary>
        /// <param name="adVehicleId"></param>
        /// <returns></returns>
        Task<AdVehicle> GetAsync(long adVehicleId);
        IEnumerable<object> GetCountOfAdsObject(long userId);
        IEnumerable<object> GetCountOfCategories(long userId, Enums.RoomSorter sorter);

        /// <summary>
        /// Get total count of items by queries params
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string queryString, long regionId = 0, long cityId = 0, long page = 1);

        /// <summary>
        ///  Get total count of items by KeyValuePair params
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="page"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(long regionId = 0, long cityId = 0, long page = 1,
            params KeyValuePair<string, string>[] @params);

        /// <summary>
        /// When you need parse params from url raw query string. Usually for GET query.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<IEnumerable<AdVehicle>> GetAsync(string queryString, long regionId, long cityId, long page = 1);

        /// <summary>
        /// Used for POST queries
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="cityId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<IEnumerable<AdVehicle>> GetAsync(long regionId, long cityId, long pageNumber, params KeyValuePair<string, string>[] @params);

        Task<int> GetCountByDealershipIdAsync(long dealershipId);

        Task<IEnumerable<AdVehicle>> GetByDealershipIdAsync(long dealershipId, int pageNumber, int pageSize, Currency currency = null, PriceSorter? sorter = null);
        Task<IEnumerable<AdVehicle>> GetForSiteMapAsync(Expression<Func<AdVehicle, bool>> predicate);
    }
}