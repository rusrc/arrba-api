using System;
using System.Collections.Generic;
using Arrba.Domain.Models;

namespace Arrba.DTO
{
    public class VehicleDto
    {
        public long ID { get; set; }
        public long CategID { get; set; }
        public string Year { get; set; }
        public double Price { get; set; }
        public double? MinimalPrice { get; set; }
        public bool HotSelling { get; set; }
        public string ConvertedCurrencySymbol { get; set; }
        public string ConvertedPrice { get; set; }
        public string OtherConvertedPrice { get; set; }
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string Description { get; set; }
        public string FolderImgName { get; set; }
        public long ViewCount { get; set; }
        public bool ImgExists { get; set; }

        public string DealershipName { get; set; }
        public string DealershipId { get; set; }

        public string MainImg { get; set; }
        public string MainImgMiddle { get; set; }
        public string MainImgSuperSmall { get; set; }
        public object MapJsonCoordList { get; set; }
        public string ShortDescription { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime DateExpired { get; set; }
        public bool IsExpired { get; set; }
        public string CityName { get; set; }
        public string CityAlias { get; set; }
        public string CityNameMultiLang { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyFullName { get; set; }
        public string CurrencySymbol { get; set; }
        public string BrandName { get; set; }
        public string TypeName { get; set; }
        public string ModelName { get; set; }
        public string CategName { get; set; }
        public string CategAlias { get; set; }
        public string CategNameMultiLang { get; set; }
        public string IsAutoUpdatable { get; set; }
        public string ModirationStatus { get; set; }
        public string AdStatus { get; set; }
        public string Condition { get; set; }
        public string CommentRestriction { get; set; }
        public string Comment { get; set; }
        public string ImagePath { get; set; }
        public ICollection<AdVehicleServiceStore> Services { get; set; }
    }
}
