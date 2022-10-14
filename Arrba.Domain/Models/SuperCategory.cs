using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Arrba.Domain.Models
{
    public class SuperCategory : MultiLangPropertyExtension
    {
        public long ID { get; set; }

        public string Alias { get; set; }

        public int Order { get; set; }

        public override string Name { get; set; }

        public ActiveStatus Status { get; set; }
        public SuperCategType SuperCategType { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdVehicle> AdVehicles { get; set; }

        #region not mapped properties

        [NotMapped]
        public string Index { get; set; }

        [NotMapped]
        public string Controller { get; set; }

        #endregion
    }

    public class SuperCategoryConfiguration : IEntityTypeConfiguration<SuperCategory>
    {
        public void Configure(EntityTypeBuilder<SuperCategory> builder)
        {
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.Alias).HasMaxLength(180);
        }
    }
}