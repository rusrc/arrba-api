using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Arrba.Domain;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;
using Arrba.Extensions;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    using SETTS = Constants.SETTINGS;
    public partial class QueryString : IQueryString
    {
        private readonly DbArrbaContext _ctx;
        /// <summary>
        /// The constructor get query url string and context to generate object with main props
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="ctx"></param>
        public QueryString(DbArrbaContext ctx)
        {
            this._ctx = ctx;
        }
        /// <summary>
        /// Main parameterized url query. Example: ?param1=1&param2=value
        /// Основная строка запроса в url
        /// </summary>
        public string QueryUrlString { get; set; }
        /// <summary>
        /// Получить данные в поисковой строки для запросов типа like
        /// </summary>
        public string SearchText
        {
            get
            {
                this.CheckQueryUrlString();
                var regexp = new Regex(@"SearchText=[\w\s\d\%\+]*", RegexOptions.IgnoreCase);
                return regexp.Match(QueryUrlString).Value;
            }
        }
        public Dictionary<string, string> QueryStringCollection
        {
            get
            {
                this.CheckQueryUrlString();

                QueryUrlString = QueryUrlString.Replace("?", "").Replace("=on", "=1");

                string[] arr = QueryUrlString.Split('&');

                return arr.Select(item => item.Split('='))
                            .Where(itemArr => !string.IsNullOrEmpty(itemArr[1]))
                            .ToDictionary(itemArr => itemArr[0], itemArr => itemArr[1]);
            }
        }
        public Dictionary<string, long> QueryLongCollection
        {
            get
            {
                this.CheckQueryUrlString();

                QueryUrlString = QueryUrlString.Replace("?", "").Replace("=on", "=1");

                string[] arr = QueryUrlString.Split('&');

                return arr.Select(item => item.Split('='))
                    .Where(itemArr => !string.IsNullOrEmpty(itemArr[1]) && itemArr[1].IsNumeric())
                    .ToDictionary(
                        itemArr => itemArr[0],
                        itemArr => long.Parse(itemArr[1])
                    );
            }
        }
        /// <summary>
        /// DynamicProperties as typeof checkbox, select, value
        /// </summary>
        public List<PropertyModelView> DynamicProperties
        {
            get
            {
                var keyValueCollection = this.QueryStringCollection;
                var keyValueFromToCollection = this.MkFromToDictionary(keyValueCollection);

                keyValueCollection = keyValueCollection
                    .Union(keyValueFromToCollection)
                    .ToDictionary(a => a.Key, a => a.Value);

                var regexp = new Regex("^\\d+");
                var propList =
                    (from dp in keyValueCollection
                    .Where(e => regexp.Match(e.Key).Success)
                    .Select(e => new
                    {
                        PropertyID = long.Parse(e.Key),
                        Value = e.Value
                    })
                     join p in _ctx.Properties
                     on dp.PropertyID equals p.ID
                     select new PropertyModelView
                     {
                         ID = p.ID,
                         Name = p.Name,
                         ActiveStatus = p.ActiveStatus,
                         Value = dp.Value,
                         ControlType = p.ControlType,
                         Description = p.Description,
                         PropertyGroupID = p.PropertyGroupID
                     })
                    .ToList();

                return propList;
            }

        }

        /// <summary>
        /// Make dictionary with values like ("5" => "100|250")
        /// from dictionary as:
        /// _dic.Add("p_from_5", "100");
        /// _dic.Add("p_to_5", "250");
        /// </summary>
        /// <param name="dicWithFromToKeys">The dictionary where should to looking for paars with (from and to) marks</param>
        /// <param name="patternFromTo">Regexp pattern to find marks like "from_5" and "to_5"</param>
        /// <returns>Dictionary with values like [from|to] or "100|250"</returns>
        public Dictionary<string, string> MkFromToDictionary(Dictionary<string, string> dicWithFromToKeys, string patternFromTo = "(p_from_|p_to_)\\d+")
        {
            var fromToIDs = dicWithFromToKeys
                               .Where(e => new Regex(patternFromTo, RegexOptions.IgnoreCase).Match(e.Key).Success)
                               .Select(x => new Regex("\\d+").Match(x.Key).Value)
                               .Distinct();


            var resultedDic = new Dictionary<string, string>();
            foreach (string num in fromToIDs)
            {
                var @from = dicWithFromToKeys.SingleOrDefault(d => d.Key == string.Concat("p_from_", num));
                var @to = dicWithFromToKeys.SingleOrDefault(d => d.Key == string.Concat("p_to_", num));

                if (@from.Value != null && @to.Value != null)
                {
                    resultedDic.Add(num, string.Concat(from.Value, "|", to.Value));
                }
            }


            return resultedDic;
        }
        /// <summary>
        /// Get value by key name from url query string
        /// e.g. TypeID=5 returns 5 if key is "TypeID"
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private long GetProp(string key)
        {
            KeyValuePair<string, string> keyValue = 
                QueryStringCollection
                .SingleOrDefault(e => e.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(keyValue.Value))
            {
                string value = keyValue.Value;
                if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    value = "1";
                }
                if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                {
                    value = "0";
                }

                return long.Parse(value);
            }

            return 0;
        }
        /// <summary>
        /// Check if query have to be cached. 
        /// Immplement here what kind of params should to be
        /// in order to do caching. 
        /// For example: query has just categoryId it is good to cache as it is too often query.
        /// If query more complex let's fetch the data from database directly.
        /// </summary>
        /// <returns></returns>
        public bool IsQueryCached()
        {
            var result = QueryStringCollection
                .Where(e => new string[] { "SUPERCATEGID", "CATEGID", "PAGENUMBER" }
                .Contains(e.Key.ToUpper()));

            var isTrue = result.Count() == QueryStringCollection.Count();

            return isTrue;
        }

        private void CheckQueryUrlString()
        {
            if (string.IsNullOrEmpty(QueryUrlString))
            {
                throw new NullReferenceException("Please provide the '" + nameof(QueryUrlString) + "' property by value");
            }
        }

    }

    public partial class QueryString
    {
        private long _offsetCountRows;
        private long _countRows;
        private long _pageNumber;
        public long SuperCategID => GetProp("SuperCategID");
        public long CategID => GetProp("CategID");
        public long TypeID => GetProp("TypeID");
        public long BrandID => GetProp("BrandID");
        public long ModelID => GetProp("ModelID");
        public long CurrencyID
        {
            get
            {

                var currencyId = GetProp("CurrencyID");
                currencyId = currencyId <= 0 ? SETTS.DefaultCurrencyID : currencyId;

                return currencyId;
            }
            set { }
        }
        public long FromPrice => GetProp("priceFrom");
        public long ToPrice => GetProp("priceTo");
        public long FromYear => GetProp("yearFrom");
        public long ToYear => GetProp("yearTo");
        public long HasPhoto => GetProp("HasPhoto");
        public long HotSelling => GetProp("HotSelling");
        public long CustomClear => GetProp("CustomClear");
        public long InInstalments => GetProp("InInstalments");
        public long ExchangePossible => GetProp("ExchangePossible");
        public long ForceCityID => GetProp("_ForceCityID");
        public PriceSorter PriceSorter => (PriceSorter)GetProp("priceSorter");
        public VehicleCondition Condition => (VehicleCondition)GetProp("vehicleCondition");

        //TODO Add default value before get e.g. - .OffsetCountRows = SETTS.CountRows * repo.QueryString.PageNumber - SETTS.CountRows;
        public long OffsetCountRows
        {
            get { return this._offsetCountRows > 0 ? this._offsetCountRows : GetProp("OffsetCountRows"); }
            set { this._offsetCountRows = value; }
        }
        //TODO Add default value before get e.g. - .QueryString.CountRows = SETTS.CountRows;
        public long CountRows
        {
            get { return this._countRows > 0 ? this._countRows : GetProp("CountRows"); }
            set { this._countRows = value; }
        }
        public long PageNumber
        {
            get
            {
                long pageNumber = GetProp("PageNumber");
                if (this._pageNumber > 0)
                    return this._pageNumber;
                if (pageNumber > 0)
                    return pageNumber;
                return 1;
            }
            set { this._pageNumber = value; }
        }
    }
}