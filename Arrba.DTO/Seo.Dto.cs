using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Arrba.DTO
{
    public class Seo
    {
        public Seo() { }
        public Seo(params string[] metaKeys)
        {
            var result = metaKeys.Where(m => !string.IsNullOrEmpty(m)).Aggregate((first, prev) => $"{first}, {prev}");
            this.MetaKeys = result;
        }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("h1")]
        public string H1 { get; set; }
        [JsonProperty("metaDescription")]
        public string MetaDescription { get; set; }
        [JsonProperty("metaKeys")]
        public string MetaKeys { get; set; }
        [JsonProperty("breadcrumbs")]
        public IEnumerable<KeyValuePair<string, string[]>> Breadcrumbs { get; set; }
    }
}
