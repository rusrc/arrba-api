using System.ComponentModel.DataAnnotations;
using Arrba.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arrba.DTO
{
    public class AddNewVehicleDto
    {
        [Required]
        [JsonProperty(PropertyName = "superCategoryId")]
        public long SuperCategoryId { get; set; }
        [Required]
        [JsonProperty(PropertyName = "categoryId")]
        public long CategoryId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "TypeId")]
        public long TypeId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "BrandId")]
        public long BrandId { get; set; }

        [JsonProperty(PropertyName = "ModelId")]
        public long? ModelId { get; set; }

        [JsonProperty(PropertyName = "modelValue")]
        public string ModelValue { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty(PropertyName = "CurrencyId")]
        public long CurrencyId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "CountryId")]
        public long CountryId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "CityId")]
        public long CityId { get; set; }

        [JsonProperty(PropertyName = "additionalComment")]
        public string AdditionalComment { get; set; }

        [JsonProperty(PropertyName = "commentMode")]
        public string CommentMode { get; set; }

        [Required]
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "minimalPrice")]
        public double? MinimalPrice { get; set; }

        // [Required]
        [JsonProperty(PropertyName = "year")]
        public string Year { get; set; }

        [JsonProperty(PropertyName = "phoneNumbers")]
        public string[] PhoneNumbers { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public JObject Properties { get; set; }

        [JsonProperty(PropertyName = "temporaryImageFolder")]
        public string TemporaryImageFolder { get; set; }

        [JsonProperty(PropertyName = "dealershipId")]
        public long? DealershipId { get; set; }

        [JsonProperty(PropertyName = "mapJsonCoord")]
        public string MapJsonCoord { get; set; }

        public VehicleCondition Condition { get; set; }

        public bool IsDealer => DealershipId.HasValue;
    }
}
