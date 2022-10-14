using System.Collections.Generic;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Region : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public long CountryID { get; set; }
        public override string Name { get; set; }
        public string Alias { get; set; }


        [JsonIgnore]
        public virtual Country Country { get; set; }

        [JsonIgnore]
        public virtual ICollection<City> Cities { get; set; }
        [JsonIgnore]
        public virtual ICollection<AdVehicle> AdVehicles { get; set; }
    }
}