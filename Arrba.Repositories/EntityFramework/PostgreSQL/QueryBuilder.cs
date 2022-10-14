using System;
using System.Text;
using Arrba.Domain.Models;
using Arrba.Extensions;
using Arrba.Repositories.Enums;
using Arrba.Services.Logger;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    /// <summary>
    /// Формирование запроса на выборку объявлений рекреационной техники
    /// </summary>
    public class QueryBuilder : IQueryBuilder
    {
        public Enums.TypeField TypeField { get; set; }
        private IQueryString _queryString { get; set; }
        // private static Lazy<LogService> LogService => new Lazy<LogService>(() => new LogService());

        /// <summary>
        /// If QueryString object has OffsetCountRows or CountRows values cunstructor set them atherwise 0;
        /// </summary>
        /// <param name="queryString"><see cref="IQueryBuilder"/></param>
        /// <param name="cityID">city id</param>
        /// <param name="regionID">regoin id</param>
        /// <param name="countryID">country id</param>
        public QueryBuilder(IQueryString queryString)
        {
            this._queryString = queryString;
        }

        /// <summary>
        /// Запрос, который формируется (из основного запроса, 
        /// запроса на порядом, запроса из динамических свойсв) |
        /// Query that form from (main query, order query, dynamic properties query)
        /// </summary>
        public string Select(long countryId, long regionId = 0, long cityId = 0)
        {
            if (countryId == 0)
            {
                throw new ApplicationException("Please provide the default countryId");
            }

            string properties = this.AddProperties(),
                   orderBy = this.AddOrdering(),
                   offset = string.Empty;

            if (this._queryString.OffsetCountRows != 0 || this._queryString.CountRows != 0)
            {
                offset = this.AddOffsetString(this._queryString.OffsetCountRows, this._queryString.CountRows);
            }

            string head = "*";
            switch (this.TypeField)
            {
                case TypeField.SelectCount:
                    head = "COUNT(*)";
                    break;
                case TypeField.SelectIds:
                    head = $@"""{nameof(AdVehicle.ID)}""";
                    break;
            }

            var query = $@"
                SELECT {head} FROM (
	                SELECT DISTINCT {this.Fields(_queryString.CurrencyID, tblAlias: "adv")} FROM ""AdVehicles"" adv
	                {(!string.IsNullOrEmpty(properties) ? "JOIN \"DynamicPropertyAdVehicles\" da ON adv.\"ID\" = da.\"AdVehicleID\"" : "")}
	                WHERE 
	                adv.""SuperCategID"" = {_queryString.SuperCategID}  
                    {(_queryString.CategID != 0 ? $"AND adv.\"CategID\"={_queryString.CategID}" : "")} 
                    {(_queryString.TypeID != 0 ? $"AND adv.\"TypeID\"={_queryString.TypeID}" : "")}
                    {(_queryString.BrandID != 0 ? $"AND adv.\"BrandID\"={_queryString.BrandID}" : "")} 
                    {(_queryString.ModelID != 0 ? $"AND adv.\"ModelID\"={_queryString.ModelID}" : "")} 
                    /* Additional properties */
                    {(_queryString.HasPhoto != 0 ? $"AND adv.\"ImgExists\"={(int)_queryString.HasPhoto}" : "")} 
                    {(_queryString.Condition != 0 ? $"AND adv.\"Condition\"={(int)_queryString.Condition}" : "")} 
                    {(_queryString.HotSelling != 0 ? $"AND adv.\"HotSelling\"={_queryString.HotSelling}" : "")} 
                    {(_queryString.CustomClear != 0 ? $"AND adv.\"CustomsCleared\"={_queryString.CustomClear}" : "")} 
                    {(_queryString.InInstalments != 0 ? $"AND adv.\"InstalmentSelling\"={_queryString.InInstalments}" : "")}
                    {(_queryString.ExchangePossible != 0 ? $"AND adv.\"ExchangePossible\"={_queryString.ExchangePossible}" : "")}  
                    /* Year in query */
                    {(_queryString.ToYear != 0 ? $"AND (adv.\"Year\" BETWEEN {_queryString.FromYear} AND {_queryString.ToYear})" : "")}    
                    {(_queryString.FromYear != 0 && _queryString.ToYear == 0 ? $"AND (adv.\"Year\" BETWEEN {_queryString.FromYear} AND {DateTime.Now.Year})" : "")}
                    /* Country, Region, City */
                    {(cityId != 0 ? $"AND (adv.\"CityID\" = {cityId})" : "")}
                    {(regionId != 0 ? $"AND (adv.\"RegionID\" = {regionId})" : "")}
                    {(countryId != 0 ? $"AND (adv.\"CountryID\" = {countryId})" : "")}
                    {properties}
                    /* Modiration varification */
                    AND (adv.""ModirationStatus"" = {(int)ModirationStatus.Ok}) 
                    /* Date expired varification uncomment if needed */
                    -- AND (adv.""DateExpired"" > GETDATE())
                ) AS T
                /* The price in query */
                {(_queryString.ToPrice != 0 ? $"WHERE (T.\"Price\" between {_queryString.FromPrice} AND {_queryString.ToPrice})" : "")}
                {(_queryString.FromPrice != 0 && _queryString.ToPrice == 0 ? $"WHERE (T.\"Price\" >= {_queryString.FromPrice})" : "")}
                /* Ordering */
                {(this.TypeField == TypeField.SelectCount ? "" : orderBy)}
                /* offset if needed */
                {offset}";

            return query;
        }

        /// <summary>
        /// Fields creation from <see cref="AdVehicle"/>
        /// </summary>
        /// <param name="tblAlias">Table alias like "SELECT * FORM AdVehicle AS Alias"</param>
        /// <returns></returns>
        public string Fields(long curreyncyId, string tblAlias = "adv")
        {
            if (TypeField == TypeField.SelectIds)
            {
                return
                    $@"{tblAlias}.""{nameof(AdVehicle.ID)}"",
                       {tblAlias}.""{nameof(AdVehicle.AddDate)}"",
                       ""ConvertCurrencyPrice""({curreyncyId}, {tblAlias}.""{nameof(AdVehicle.CurrencyID)}"", {tblAlias}.""{nameof(AdVehicle.Price)}"", (select array_to_json(array_agg(T)) from (select * from ""ExchangeRateTableView"") T)) ""{nameof(AdVehicle.Price)}""";
            }

            return $@"
                {tblAlias}.""{nameof(AdVehicle.ID)}"",
                {tblAlias}.""{nameof(AdVehicle.UserID)}"", 
                {tblAlias}.""{nameof(AdVehicle.SuperCategID)}"", 
                {tblAlias}.""{nameof(AdVehicle.CategID)}"", 
                {tblAlias}.""{nameof(AdVehicle.BrandID)}"", 
                {tblAlias}.""{nameof(AdVehicle.ModelID)}"", 
                {tblAlias}.""{nameof(AdVehicle.TypeID)}"", 
                {tblAlias}.""{nameof(AdVehicle.CityID)}"", 
                {tblAlias}.""{nameof(AdVehicle.RegionID)}"", 
                {tblAlias}.""{nameof(AdVehicle.CountryID)}"", 
                {tblAlias}.""{nameof(AdVehicle.Year)}"", 
                {tblAlias}.""{nameof(AdVehicle.NewModelName)}"", 
                ""ConvertCurrencyPrice""({curreyncyId}, {tblAlias}.""{nameof(AdVehicle.CurrencyID)}"", {tblAlias}.""{nameof(AdVehicle.Price)}"", (select array_to_json(array_agg(T)) from (select * from ""ExchangeRateTableView"") T)) ""{nameof(AdVehicle.Price)}"",
                {tblAlias}.""{nameof(AdVehicle.Title)}"",
	            {tblAlias}.""{nameof(AdVehicle.CommentRestriction)}"", 
                {tblAlias}.""{nameof(AdVehicle.Description)}"", 
                {tblAlias}.""{nameof(AdVehicle.ModelVerification)}"", 
                {tblAlias}.""{nameof(AdVehicle.CurrencyID)}"", 
                {tblAlias}.""{nameof(AdVehicle.ViewCount)}"", 
                {tblAlias}.""{nameof(AdVehicle.DateExpired)}"", 
                {tblAlias}.""{nameof(AdVehicle.ModirationStatus)}"", 
                {tblAlias}.""{nameof(AdVehicle.IsAutoUpdatable)}"", 
                {tblAlias}.""{nameof(AdVehicle.AdStatus)}"", 
                {tblAlias}.""{nameof(AdVehicle.AddDate)}"", 
                {tblAlias}.""{nameof(AdVehicle.LastModified)}"", 
                {tblAlias}.""{nameof(AdVehicle.FolderImgName)}"", 
                {tblAlias}.""{nameof(AdVehicle.MapJsonCoord)}"", 
                {tblAlias}.""{nameof(AdVehicle.ImgJson)}"", 
                {tblAlias}.""{nameof(AdVehicle.Condition)}"", 
                {tblAlias}.""{nameof(AdVehicle.InstalmentSelling)}"", 
                {tblAlias}.""{nameof(AdVehicle.CustomsCleared)}"",
                {tblAlias}.""{nameof(AdVehicle.HotSelling)}"", 
                {tblAlias}.""{nameof(AdVehicle.ExchangePossible)}"", 
                {tblAlias}.""{nameof(AdVehicle.ImgExists)}"",
                {tblAlias}.""{nameof(AdVehicle.DealershipId)}""";
        }


        /// <summary>
        /// Подзапрос на выборку по динамическим свойствам |
        /// Subquery to select ads by dynamic properties 
        /// </summary>
        /// <returns>Sql query as string</returns>
        private string AddProperties()
        {
            var stringBuilder = new StringBuilder();
            var i = 0;

            foreach (var prop in this._queryString.DynamicProperties)
            {
                var orAnd = i > 0 ? "OR" : "";
                switch (prop.ControlType)
                {
                    case ControlTypeEnum.Value:
                        stringBuilder.AppendFormat("{2} da.\"PropertyID\"  = {0} AND da.\"PropertyValue\" <= CAST('{1}' AS NUMERIC)\r\n", prop.ID, prop.Value, orAnd);
                        break;
                    case ControlTypeEnum.ValueFromTo:

                        var from = prop.Value.ToDouble(0);
                        var to = prop.Value.ToDouble(1);
                        string s;

                        if (to == 0)
                            s = "{2} da.\"PropertyID\" = {0} AND CAST(da.\"PropertyValue\" AS NUMERIC) >= {1} /*{3}*/\r\n";
                        else
                            s = "{2} da.\"PropertyID\" = {0} AND CAST(da.\"PropertyValue\" AS NUMERIC) BETWEEN {1} AND {3}\r\n";

                        stringBuilder.AppendFormat(s, prop.ID, from, orAnd, to);

                        break;
                    case ControlTypeEnum.Select:
                        stringBuilder.AppendFormat("{2} da.\"PropertyID\"  = {0} AND da.\"PropertyValue\" = '{1}'\r\n", prop.ID, prop.Value, orAnd);
                        break;
                    case ControlTypeEnum.CheckBox:
                        stringBuilder.AppendFormat("{2} da.\"PropertyID\"  = {0} AND da.\"PropertyValue\" = '{1}'\r\n", prop.ID, prop.Value, orAnd);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                i++;
            }

            //Подзапрос
            var result = $@"
                    AND adv.""ID"" in (
					    select da.""AdVehicleID"" from ""DynamicPropertyAdVehicles"" da
					    where 
					    {stringBuilder}
					    group by da.""AdVehicleID""
					    having count(*) = {this._queryString.DynamicProperties.Count}
			   )";

            return stringBuilder.ToString() != string.Empty ? result : "";

        }


        /// <summary>
        /// Небольшая порция запроса на упорядочивание |
        /// Small partial string of ORDER BY query
        /// </summary>
        /// <returns>Sql query as string</returns>
        private string AddOrdering()
        {
            switch (_queryString.PriceSorter)
            {
                case PriceSorter.PriceDesc:
                    return "ORDER BY T.\"Price\" DESC";
                case PriceSorter.PriceAsc:
                    return "ORDER BY T.\"Price\" ASC";
                case PriceSorter.DateDesc:
                    return "ORDER BY T.\"AddDate\" DESC";
                case PriceSorter.DateAsc:
                    return "ORDER BY T.\"AddDate\" ASC";
                default:
                    return "ORDER BY T.\"AddDate\" DESC";
            }
        }

        /// <summary>
        /// Add skip take mechanism
        /// </summary>
        /// <param name="offsetCountRows">Count of rows that should to by offseted</param>
        /// <param name="countRows">Count of rows after Offset</param>
        /// <returns></returns>
        private string AddOffsetString(long offsetCountRows, long countRows)
        {
            string result = $@"OFFSET {offsetCountRows} ROWS 
                               FETCH NEXT {countRows} ROWS ONLY";


            return result;
        }
    }
}