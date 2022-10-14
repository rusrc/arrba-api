using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Arrba.Constants;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Exceptions;
using Arrba.Repositories.Enums;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class AdVehicleRepository : Repository<AdVehicle>, IAdVehicleRepository, IDisposable
    {
        private readonly Expression<Func<AdVehicle, bool>> _onModirationExpression = vehicle =>
            vehicle.ModirationStatus == ModirationStatus.Required ||
            vehicle.ModelVerification == AdNeedModelVerification.Required;

        private readonly Expression<Func<AdVehicle, bool>> _activeExpression = vehicle =>
            vehicle.DateExpired >= DateTime.Now &&
            vehicle.ModirationStatus == ModirationStatus.Ok &&
            vehicle.ModelVerification == AdNeedModelVerification.Ok;

        private readonly Expression<Func<AdVehicle, bool>> _archiveExpression =
            vehicle => vehicle.DateExpired < DateTime.Now;

        private readonly IQueryString _queryString;
        private readonly ISearchFormRepository _searchFormRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public AdVehicleRepository(
            DbArrbaContext context,
            IQueryString queryString,
            ISearchFormRepository searchFormRepository,
            ICurrencyRepository currencyRepository) : base(context)
        {
            this._queryString = queryString;
            this._searchFormRepository = searchFormRepository;
            this._currencyRepository = currencyRepository;
        }

        public IQueryable<AdVehicle> GetAsync(long userId, long categoryId, RoomSorter sorter)
        {
            IQueryable<AdVehicle> query = AdVehicles(userId);

            switch (sorter)
            {
                case RoomSorter.All: break;
                case RoomSorter.InArchive: return query.Where(_archiveExpression);
                case RoomSorter.OnModiration: return query.Where(_onModirationExpression);
                case RoomSorter.Active: return query.Where(_activeExpression);
            }

            if (categoryId > 0)
            {
                query = query.Where(adVehicle => adVehicle.CategID == categoryId);
            }

            return query;
        }

        public IEnumerable<object> GetCountOfAdsObject(long userId)
        {
            var adVechicles = _context.AdVehicles
                .Where(a => a.UserID == userId && a.AdStatus != AdStatus.DeleteForever);

            int allAds = adVechicles.Count();
            int active = adVechicles.Count(_activeExpression);
            int inArchive = adVechicles.Count(_archiveExpression);
            int onModiration = adVechicles.Count(_onModirationExpression);

            var list = new List<object>
            {
                new {Key = "all", Value = allAds},
                new {Key = "active", Value = active},
                new {Key = "inArchive", Value = inArchive},
                new {Key = "onModiration", Value = onModiration}
            };

            return list;
        }

        public IEnumerable<object> GetCountOfCategories(long userId, RoomSorter sorter)
        {
            IQueryable<AdVehicle> adVechicles = _context.AdVehicles
                .Where(a => a.UserID == userId && a.AdStatus != AdStatus.DeleteForever);

            switch (sorter)
            {
                case RoomSorter.InArchive:
                    adVechicles = adVechicles.Where(_archiveExpression);
                    break;
                case RoomSorter.OnModiration:
                    adVechicles = adVechicles.Where(_onModirationExpression);
                    break;
                case RoomSorter.Active:
                    adVechicles = adVechicles.Where(_activeExpression);
                    break;
            }

            return adVechicles
                .Include(adVehicle => adVehicle.Categ)
                .GroupBy(adVehicle => new
                {
                    adVehicle.Categ.Name,
                    adVehicle.CategID
                })
                .Select(g => new
                {
                    CategoryId = g.Key.CategID,
                    Key = g.Key.Name,
                    Value = g.Count()
                })
                .ToList();
        }

        public virtual async Task<AdVehicle> GetAsync(long adVehicleId)
        {
            var adVehicle = await
                _context.AdVehicles
                .Include(e => e.Currency)
                .Include(e => e.Services)
                .Include(e => e.City)
                .Include(e => e.Categ)
                .Include(e => e.Type)
                .Include(e => e.Brand)
                .Include(e => e.Model)
                .Include(e => e.Dealership)
                .Include(e => e.DynamicPropertyAds)
                .ThenInclude(da => da.Property)
                .ThenInclude(p => p.SelectOptions)
                .SingleOrDefaultAsync(a => a.ID == adVehicleId && a.AdStatus != AdStatus.DeleteForever);

            var rates = this._currencyRepository.GetExchangeRates();
            await this.AddCovertedPrice(adVehicle, adVehicle?.Currency, rates);

            return adVehicle;
        }

        public async Task<int> GetCountAsync(string queryString, long regionId = 0, long cityId = 0, long page = 1)
        {
            long countryId = SETTINGS.DefaultCountryId;

            _queryString.QueryUrlString = queryString;

            var resultedList =
                await _searchFormRepository.GetCountAsync(_queryString, countryId, regionId, cityId);

            return resultedList;
        }

        public async Task<int> GetCountAsync(long regionId = 0, long cityId = 0, long page = 1, params KeyValuePair<string, string>[] @params)
        {
            long countryId = SETTINGS.DefaultCountryId;

            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (@params.Length > 0)
            {
                foreach (KeyValuePair<string, string> keyValue in @params)
                {
                    query.Add(keyValue.Key, keyValue.Value);
                }
            }

            _queryString.QueryUrlString = query.ToString();

            var resultedList = await _searchFormRepository.GetCountAsync(_queryString, countryId, regionId, cityId);

            return resultedList;
        }

        public async Task<IEnumerable<AdVehicle>> GetAsync(string queryString, long regionId, long cityId, long page = 1)
        {
            long countryId = SETTINGS.DefaultCountryId;

            _queryString.QueryUrlString = queryString;
            _queryString.CountRows = SETTINGS.CountRows;
            _queryString.OffsetCountRows = page * SETTINGS.CountRows - SETTINGS.CountRows;

            var vehicles = await _searchFormRepository.GetAdsAsync(_queryString, countryId, regionId, cityId);

            if (_queryString.CurrencyID > 0)
            {
                var requestedCurrency = await this._currencyRepository.GetAsync(_queryString.CurrencyID);
                if (requestedCurrency == null)
                {
                    throw new BusinessLogicException($"Can't get requested currency by id: {_queryString.CurrencyID}");
                }

                var exchangeRates = await this._currencyRepository.GetExchangeRatesAsync();
                foreach (var vehicle in vehicles)
                {
                    await this.AddCovertedPrice(vehicle, requestedCurrency, exchangeRates);
                }
            }

            return vehicles;
        }

        public async Task<IEnumerable<AdVehicle>> GetAsync(long regionId, long cityId, long pageNumber, params KeyValuePair<string, string>[] @params)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (@params.Length > 0)
            {
                foreach (KeyValuePair<string, string> keyValue in @params)
                {
                    query.Add(keyValue.Key, keyValue.Value);
                }
            }

            return await GetAsync(query.ToString(), regionId, cityId, pageNumber);
        }

        public async Task<int> GetCountByDealershipIdAsync(long dealershipId)
        {
            var count = _context.AdVehicles
                .Where(v => v.AdStatus != AdStatus.DeleteForever)
                .CountAsync(v => v.DealershipId == dealershipId);

            return await count;
        }

        public async Task<IEnumerable<AdVehicle>> GetByDealershipIdAsync(long dealershipId, int pageNumber, int pageSize,
            Currency currency = null, PriceSorter? sorter = null)
        {
            var query = _context.AdVehicles
                .Include(v => v.Categ)
                .Include(v => v.Currency)
                .Include(v => v.City)
                .Include(v => v.Type)
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.Services)
                .AsQueryable();

            switch (sorter)
            {
                case PriceSorter.PriceAsc:
                    query = query.OrderBy(v => v.Price);
                    break;
                case PriceSorter.PriceDesc:
                    query = query.OrderByDescending(v => v.Price);
                    break;
                case PriceSorter.DateDesc:
                    query = query.OrderByDescending(v => v.AddDate);
                    break;
                default:
                    query = query.OrderBy(v => v.AddDate);
                    break;
            }

            query = query
             .Where(v => v.AdStatus != AdStatus.DeleteForever)
             .Where(v => v.DealershipId == dealershipId);

            query = pageNumber == 1
                ? query.Skip(0).Take(pageSize)
                : query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var vehicles = await query.ToListAsync();
            var exchangeRates = await this._currencyRepository.GetExchangeRatesAsync();

            vehicles.ForEach(async vehicle => await AddCovertedPrice(vehicle, currency, exchangeRates));

            return vehicles;
        }

        public async Task<IEnumerable<AdVehicle>> GetForSiteMapAsync(Expression<Func<AdVehicle, bool>> predicate)
        {
            var items = await _context.AdVehicles
                .Include(v => v.Categ)
                .Include(v => v.Brand)
                .Include(v => v.Type)
                .Include(v => v.Model)
                .Include(v => v.City)
                .ToListAsync();

            return items;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // TODO need refactoring
        private async Task AddCovertedPrice(AdVehicle vehicle, Currency requestedCurrency, IEnumerable<ExchangeRate> exchangeRates)
        {
            requestedCurrency = requestedCurrency ?? await this._currencyRepository.GetAsync(SETTINGS.DefaultCurrencyID);
            // if currency of item differ from requestedCurrency next convert item currency (convertedCurrency)
            // if currency the same do nothing just convert forman to .ToString("### ### ### ###")
            if (vehicle.CurrencyID != requestedCurrency.ID)
            {
                var rate = exchangeRates.SingleOrDefault(ex => ex.TargetCurrencyId == requestedCurrency.ID && ex.SourceCurrencyId == vehicle.CurrencyID).Rate;
                vehicle.ConvertedCurrencySymbol = requestedCurrency.Symbol;
                vehicle.ConvertedPrice = /*~*/ $"{vehicle.Price * rate:### ### ### ### ###}";

                // TODO Add otherConvertedPrice property
                var otherConvertedPrice = exchangeRates
                    .Where(er => er.TargetCurrencyId != requestedCurrency.ID && er.SourceCurrencyId == vehicle.CurrencyID)
                    .Select(er => /*~*/ $"{(vehicle.Price * er.Rate):### ### ### ### ###} {er.TargetName}")
                    .Aggregate((prev, next) => $"{prev}, {next}");

                vehicle.OtherConvertedPrice = otherConvertedPrice;
            }
            else
            {
                vehicle.ConvertedPrice = $"{vehicle.Price:### ### ### ### ###}";

                // TODO Add otherConvertedPrice property
                var otherConvertedPrice = exchangeRates
                    .Where(er => er.TargetCurrencyId != vehicle.CurrencyID && er.SourceCurrencyId == vehicle.CurrencyID)
                    .Select(er => /*~*/ $"{(vehicle.Price * er.Rate):### ### ### ### ###} {er.TargetName}")
                    .Aggregate((prev, next) => $"{prev}, {next}");

                vehicle.OtherConvertedPrice = otherConvertedPrice;
            }
        }
        private IQueryable<AdVehicle> AdVehicles(long userId)
        {
            var adVehicles = _context.AdVehicles
                .Include(e => e.Categ)
                .Include(e => e.Currency)
                .Include(e => e.City)
                .Include(e => e.Type)
                .Include(e => e.Brand)
                .Include(e => e.Model)
                .Include(e => e.Services)
                .Where(a => a.UserID == userId && a.AdStatus != AdStatus.DeleteForever);

            return adVehicles;
        }
    }
}
