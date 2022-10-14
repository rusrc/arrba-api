using Newtonsoft.Json;

using System.Collections.Generic;

namespace Arrba.GoogleCaptchaV2
{
    public class GrecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}