using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class PropertyCheckBoxGroup
    {
        public long PropertyID { get; set; }
        public long CheckBoxGroupID { get; set; }

        [JsonIgnore]
        public virtual Property Property { get; set; }
        public virtual CheckBoxGroup CheckBoxGroup { get; set; }
    }

    public class PropertyCheckBoxGroupConfiguration : IEntityTypeConfiguration<PropertyCheckBoxGroup>
    {
        public void Configure(EntityTypeBuilder<PropertyCheckBoxGroup> builder)
        {
            builder.HasKey(p => new { p.PropertyID, p.CheckBoxGroupID });
        }
    }
}