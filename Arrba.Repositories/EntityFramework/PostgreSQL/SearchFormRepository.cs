using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Constants;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Exceptions;
using Arrba.Repositories.Enums;
using Arrba.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class SearchFormRepository : ISearchFormRepository
    {
        private readonly DbArrbaContext ctx;
        private readonly ILogService _logService;

        public SearchFormRepository(DbArrbaContext ctx, ILogService logService)
        {
            this.ctx = ctx;
            this._logService = logService;
        }

        public virtual async Task<int> GetCountAsync(IQueryString queryString, long countryId, long regionId = 0, long cityId = 0)
        {
            cityId = queryString.ForceCityID > 0 ? queryString.ForceCityID : cityId;
            var queryBuilder = new QueryBuilder(queryString)
            {
                TypeField = TypeField.SelectCount
            };

            #region Main query for count
            var sw = new Stopwatch();
            sw.Start();

            var query = queryBuilder.Select(countryId, regionId, cityId);
            //var count = await ctx.Database.SqlQuery<int>(query).FirstOrDefaultAsync();
            var count = (await RawSqlQuery(query, reader => (long)reader[0])).FirstOrDefault();

            sw.Stop();
            this._logService.Info($"Count SELECT query take: {sw.ElapsedMilliseconds}ms, count: {count}");
            #endregion

            return (int)count;
        }

        public virtual async Task<IEnumerable<AdVehicle>> GetAdsAsync(IQueryString queryString, long countryId, long regionId = 0, long cityId = 0)
        {
            if (queryString.CountRows > 100)
            {
                throw new BusinessLogicException("If count of rows more then 100 the query could be slow");
            }

            if (queryString.CountRows > SETTINGS.CountRows)
            {
                throw new BusinessLogicException();
            }

            cityId = queryString.ForceCityID > 0 ? queryString.ForceCityID : cityId;
            var queryBuilder = new QueryBuilder(queryString)
            {
                TypeField = TypeField.SelectIds
            };

#if DEBUG
            var sw = new Stopwatch();
            sw.Start();
#endif

            var query = queryBuilder.Select(countryId, regionId, cityId);
            // var adIDs = await ctx.Database.SqlQuery<long>(query).ToArrayAsync();
            var adIDs = (await RawSqlQuery(query, reader => (long)reader[0])).ToArray();

#if DEBUG
            sw.Stop();
            this._logService.Info($"Main SELECT query take: {sw.ElapsedMilliseconds}ms");
#endif

#if DEBUG
            sw.Reset();
            sw.Start();
#endif
            var queryIncludes = ctx.AdVehicles
                .Where(vehicle => adIDs.Distinct().Contains(vehicle.ID))
                .Include(vehicle => vehicle.Brand)
                .Include(vehicle => vehicle.Model)
                .Include(vehicle => vehicle.Type)
                .Include(vehicle => vehicle.City)
                .Include(vehicle => vehicle.Currency)
                .Include(vehicle => vehicle.Categ)
                .Include(vehicle => vehicle.Services)
                .AsQueryable();

            var result = await queryIncludes.ToListAsync();

#if DEBUG
            sw.Stop();
            this._logService.Info($"Add includes SELECT query take: {sw.ElapsedMilliseconds}ms");
#endif

            return result;
        }

        private async Task<List<T>> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            var connection = ctx.Database.GetDbConnection();
            var entities = new List<T>();

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    if (connection.State == ConnectionState.Closed)
                    {
                        await connection.OpenAsync();
                    }

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            entities.Add(map(result));
                        }
                    }
                }

                return entities;
            }
            finally
            {
                // In order to avoid error `the connection pool has been exhausted` close connection
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
