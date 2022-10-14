using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class UploadImageDto
    {
        [JsonProperty("uniqueItemFolder")]
        public string UniqueItemFolder { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileNames")]
        public IEnumerable<string> FileNames { get; set; }
    }
}
