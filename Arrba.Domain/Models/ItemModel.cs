using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    [Table("Model")]
    public class ItemModel
    {
        public long ID { get; set; }
        public long CategID { get; set; }
        public long ItemTypeID { get; set; }
        public long BrandID { get; set; }
        public string Name { get; set; }
        public ActiveStatus Status { get; set; }
        public WatchWeightStatus WatchWeightStatus { get; set; }
        public long LikeValue { get; set; }
        public long LikeCount { get; set; }

        [JsonIgnore]
        public virtual Category Categ { get; set; }
        [JsonIgnore]
        public virtual ItemType ItemType { get; set; }
        [JsonIgnore]
        public virtual Brand Brand { get; set; }
        [JsonIgnore]
        public ICollection<AdVehicle> Vehicles { get; set; }
    }
}