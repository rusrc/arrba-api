using Newtonsoft.Json;
using System.Collections.Generic;

namespace Arrba.Domain.Models
{
    public class PropertyGroup : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public override string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Property> Properties { get; set; }
    }
}