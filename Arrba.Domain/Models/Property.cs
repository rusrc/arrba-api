using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class Property : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public long PropertyGroupID { get; set; }
        public override string Name { get; set; }
        public string Description { get; set; }
        public string UnitMeasure { get; set; }
        public ControlTypeEnum ControlType { get; set; }
        public ActiveStatus ActiveStatus { get; set; }


        public virtual PropertyGroup PropertyGroup { get; set; }

        public virtual ICollection<SelectOption> SelectOptions { get; set; }

        public virtual ICollection<PropertyCheckBoxGroup> PropertyCheckBoxGroups { get; set; }

        [JsonIgnore]
        public virtual ICollection<PropertyCateg> PropertyCatigs { get; set; }

        [JsonIgnore]
        public virtual ICollection<DynamicPropertyAdVehicle> DynamicPropertyAds { get; set; }
    }

    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.UnitMeasure).HasMaxLength(6);
        }
    }
}