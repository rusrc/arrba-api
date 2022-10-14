using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class ProfileDto
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("userNickName")]
        public string userName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("balanceAmount")]
        public double BalanceAmount { get; set; }
        [JsonProperty("phones")]
        public ICollection<string> Phones { get; set; }
    }
}
