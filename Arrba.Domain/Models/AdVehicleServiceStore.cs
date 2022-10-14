using System;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    /// <summary>
    ///     Tbl stores services related to ad
    /// </summary>
    public class AdVehicleServiceStore : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public long AdVehicleID { get; set; }
        public override string Name { get; set; }
        public string MetaData { get; set; }
        [JsonIgnore]
        public ActiveStatus ActiveStatus { get; set; }
        [JsonIgnore]
        public DateTime? BoughtDate { get; set; }
        [JsonIgnore]
        public DateTime? LastDate { get; set; }
        [JsonIgnore]
        public ServiceEnum ServiceType { get; set; }

        [JsonIgnore]
        public virtual AdVehicle AdVehicle { get; set; }
    }

}