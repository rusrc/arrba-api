using System;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arrba.Domain.ModelsView;
using Arrba.ImageLibrary;
using Arrba.ImageLibrary.Json;
using Arrba.ImageLibrary.ModelViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arrba.Domain.Models
{
    public class AdVehicle
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long SuperCategID { get; set; }
        public long BrandID { get; set; }
        public long TypeID { get; set; }
        public long CityID { get; set; }
        public long? RegionID { get; set; }
        public long CountryID { get; set; }
        public long CurrencyID { get; set; }
        public string Year { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public double? MinimalPrice { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NewModelName { get; set; }
        public bool InstalmentSelling { get; set; }
        public bool CustomsCleared { get; set; }
        public bool HotSelling { get; set; }
        public bool ExchangePossible { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public long CategID { get; set; }
        public long? ModelID { get; set; }
        public long? DealershipId { get; set; }
        public string ImgJson { get; set; }
        public string Description { get; set; }
        public string FolderImgName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MapJsonCoord { get; set; }
        public long ViewCount { get; set; }
        public bool ImgExists { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AddDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateExpired { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LastModified { get; set; }

        public AdStatus AdStatus { get; set; }
        public ActiveStatus IsAutoUpdatable { get; set; }
        public ModirationStatus ModirationStatus { get; set; }
        public CommentRestriction CommentRestriction { get; set; }
        public AdNeedModelVerification ModelVerification { get; set; }
        public VehicleCondition Condition { get; set; }

        public virtual SuperCategory SuperCateg { get; set; }
        public virtual Category Categ { get; set; }
        public virtual City City { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual ItemType Type { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Dealership Dealership { get; set; }
        public virtual ItemModel Model { get; set; }
        public virtual Region Region { get; set; }
        public virtual User User { get; set; }
        public ICollection<DynamicPropertyAdVehicle> DynamicPropertyAds { get; set; }
        public ICollection<AdVehicleServiceStore> Services { get; set; }

        #region NotMapped
        [NotMapped]
        public string ConvertedCurrencySymbol { get; set; }
        [NotMapped]
        public string ConvertedPrice { get; set; }
        [NotMapped]
        public string OtherConvertedPrice { get; set; }
        [NotMapped]
        public virtual List<MapJsonCoord> MapJsonCoordList
        {
            get
            {
                if (string.IsNullOrEmpty(MapJsonCoord))
                {
                    return new List<MapJsonCoord>();
                }
                return JsonConvert.DeserializeObject<List<MapJsonCoord>>(MapJsonCoord);
            }
        }
        [NotMapped]
        public virtual ImgJson ImgJsonObject
        {
            get => ImageLibrary.Json.ImgJson.Parse(this.ImgJson);
            set { }
        } 
        #endregion
    }

    public class AdVehicleConfiguration : IEntityTypeConfiguration<AdVehicle>
    {
        public void Configure(EntityTypeBuilder<AdVehicle> builder)
        {
            builder.HasOne(vehicle => vehicle.Dealership)
                 // equivalent [ForeignKey("DealershipId")]
                 .WithMany(dealership => dealership.Vehicles)
                 .HasForeignKey(vehicle => vehicle.DealershipId);

            builder.HasOne(vehicle => vehicle.Model)
                // equivalent [ForeignKey("ModelID")]
                .WithMany(model => model.Vehicles)
                .HasForeignKey(vehicle => vehicle.ModelID);

            builder.HasOne(vehicle => vehicle.Region)
                // equivalent [ForeignKey("RegionID")]
                .WithMany(region => region.AdVehicles)
                .HasForeignKey(vehicle => vehicle.RegionID);

            builder.Property(ad => ad.Comment).HasMaxLength(6000);
            builder.Property(ad => ad.Title).HasMaxLength(1024);
            builder.Property(ad => ad.Description).HasMaxLength(2048);
            builder.Property(ad => ad.FolderImgName).HasMaxLength(32);
            builder.Property(p => p.Year).HasMaxLength(4);
            builder.Property(p => p.NewModelName).HasMaxLength(100);
        }
    }
}