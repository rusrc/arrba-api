using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    [Table("Type")]
    public class ItemType : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public override string Name { get; set; }
        public string Comment { get; set; }
        public ActiveStatus Status { get; set; }

        public WatchWeightStatus WatchWeightStatus { get; set; }

        [JsonIgnore]
        public virtual ICollection<ItemModel> ItemModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<CategType> CatigTypes { get; set; }
        [JsonIgnore]
        public virtual ICollection<AdVehicle> Vehicles { get; set; }
    }

    public class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
    {
        public void Configure(EntityTypeBuilder<ItemType> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}