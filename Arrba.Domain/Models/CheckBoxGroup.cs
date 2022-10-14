using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class CheckBoxGroup : MultiLangPropertyExtension
    {
        public long ID { get; set; }
        public override string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<PropertyCheckBoxGroup> PropertyCheckBoxGroups { get; set; }
    }

    public class CheckBoxGroupConfiguration : IEntityTypeConfiguration<CheckBoxGroup>
    {
        public void Configure(EntityTypeBuilder<CheckBoxGroup> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(100);
        }
    }
}