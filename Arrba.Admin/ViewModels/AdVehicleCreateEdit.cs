using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain.Models;
using Newtonsoft.Json;

namespace Arrba.Admin.ViewModels
{
    public class AdVehicleCreateEdit
    {
        public long ID { get; set; }

        [Display(Name = "UserID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long UserID { get; set; }

        [Display(Name = "SuperCategID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long SuperCategID { get; set; }


        [Display(Name = "CategID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long CategID { get; set; }

        [Display(Name = "BrandID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long BrandID { get; set; }

        [Display(Name = "TypeID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long TypeID { get; set; }

        [Display(Name = "CityID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long CityID { get; set; }

        [Display(Name = "RegionID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long? RegionID { get; set; }

        [Display(Name = "CountryID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long CountryID { get; set; }

        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.ResourceForModelsView))]
        public long CurrencyID { get; set; }

        [Display(Name = "Year", ResourceType = typeof(Resources.ResourceForModelsView))]
        public string Year { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price", ResourceType = typeof(Resources.ResourceForModelsView))]
        public double Price { get; set; }

        [Display(Name = "MinimalPrice", ResourceType = typeof(Resources.ResourceForModelsView))]
        public double? MinimalPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NewModelName { get; set; }
        public bool InstalmentSelling { get; set; }
        public bool CustomsCleared { get; set; }
        public bool HotSelling { get; set; }
        public bool ExchangePossible { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public long? ModelID { get; set; }
        public long? DealershipId { get; set; }
        public string ImgJson { get; set; }
        public string Description { get; set; }
        public string FolderImgName { get; set; }
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
    }
}
