using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class ItemFilterDto
    {
        public long ID { get; set; }
        public long CategId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        [JsonProperty("typeOfItems")]
        public string TypeOfItems { get; set; }
    }
}
