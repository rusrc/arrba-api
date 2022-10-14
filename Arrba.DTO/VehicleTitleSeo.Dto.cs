using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class VehicleTitleSeo
    {
        [JsonProperty("brandName")]
        public string BrandName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
        [JsonProperty("cityAlias")]
        public string CityAlias { get; set; }
        [JsonProperty("typeId")]
        public long? TypeId { get; set; }
    }
}
