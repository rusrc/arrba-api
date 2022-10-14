using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class City : MultiLangPropertyExtension
    {
        [Column(Order = 0)]
        public long ID { get; set; }

        [Column(Order = 1)]
        public long RegionID { get; set; }

        [Column(Order = 2)]
        public long CountryID { get; set; }

        public override string Name { get; set; }
        public string Alias { get; set; }
        public long Weight { get; set; }
        public ActiveStatus Status { get; set; }

        [JsonIgnore]
        public virtual Country Country { get; set; }

        [JsonIgnore]
        public virtual Region Region { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdVehicle> Vehicles { get; set; }
        [JsonIgnore]
        public virtual ICollection<Dealership> Dealerships { get; set; }
    }
}