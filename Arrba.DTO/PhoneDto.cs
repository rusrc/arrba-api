using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class PhoneDto
    {
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("priorityStatus")]
        public string PriorityStatus { get; set; }
    }
}
