using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Brand
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public ActiveStatus Status { get; set; }
        public WatchWeightStatus WatchWeightStatus { get; set; }
        public long LikeValue { get; set; }
        public long LikeCount { get; set; }


        [JsonIgnore]
        public virtual ICollection<CategBrand> CategBrands { get; set; }
        [JsonIgnore]
        public virtual ICollection<ItemModel> ItemModels { get; set; }
        [JsonIgnore]
        public virtual ICollection<AdVehicle> Vehicles { get; set; }

        [NotMapped]
        public int VehiclesCount { get; set; }
    }

    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasIndex(b => b.Name).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(120);
        }
    }
}