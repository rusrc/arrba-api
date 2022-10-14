using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class DynamicPropertyAdVehicle
    {
        public long AdVehicleID { get; set; }
        public long PropertyID { get; set; }
        public string PropertyValue { get; set; }

        [JsonIgnore]
        public virtual AdVehicle AdVehicle { get; set; }

        public Property Property { get; set; }
    }

    public class DynamicPropertyAdVehicleConfiguration : IEntityTypeConfiguration<DynamicPropertyAdVehicle>
    {
        public void Configure(EntityTypeBuilder<DynamicPropertyAdVehicle> builder)
        {
            builder.HasKey(p => new { p.AdVehicleID, p.PropertyID });
            builder.Property(p => p.PropertyValue).HasMaxLength(14);
        }
    }
}