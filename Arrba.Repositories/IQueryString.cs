using System.Collections.Generic;
using Arrba.Domain.Models;
using Arrba.Domain.ModelsView;
using Arrba.Repositories.EntityFramework.PostgreSQL;

namespace Arrba.Repositories
{
    public interface IQueryString
    {
        long BrandID { get; }
        long CategID { get; }
        VehicleCondition Condition { get; }
        long CurrencyID { get; set; }
        long CustomClear { get; }
        List<PropertyModelView> DynamicProperties { get; }
        long ExchangePossible { get; }
        long ForceCityID { get; }
        long FromPrice { get; }
        long FromYear { get; }
        long HasPhoto { get; }
        long HotSelling { get; }
        long InInstalments { get; }
        long ModelID { get; }
        PriceSorter PriceSorter { get; }
        Dictionary<string, long> QueryLongCollection { get; }
        Dictionary<string, string> QueryStringCollection { get; }
        string QueryUrlString { get; set; }
        string SearchText { get; }
        long SuperCategID { get; }
        long ToPrice { get; }
        long ToYear { get; }
        long TypeID { get; }
        /// <summary>
        /// Offset count, or skip count of items
        /// </summary>
        long OffsetCountRows { get; set; }
        /// <summary>
        /// Count of rows after offset, or take count of items
        /// </summary>
        long CountRows { get; set; }
        /// <summary>
        /// Number of page
        /// </summary>
        long PageNumber { get ; set; }
        /// <summary>
        /// Check if query have to be cached or cached already. 
        /// Implement here what kind of params should to be
        /// in order to do caching. 
        /// For example: query has just categoryId it is good to cache as it is too often query.
        /// If query more complex let's fetch the data from database directly.
        /// </summary>
        /// <returns></returns>
        bool IsQueryCached();
        Dictionary<string, string> MkFromToDictionary(Dictionary<string, string> DicWithFromToKeys, string PatternFromTo = "(p_from_|p_to_)\\d+");
    }
}